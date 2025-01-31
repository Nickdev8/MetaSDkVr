using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public TMP_Text text;
    
    List<VRPlayerRig> _currentPlayers = new List<VRPlayerRig>();
    
    public SyncList<GameObject> PlayerList = new SyncList<GameObject>();

    private GameObject[] GrabbableObjects = null;

    public override void OnStartServer()
    {
        base.OnStartServer();
        AddNecessaryScripts();
    }

    private void Update()
    {
        if (!isServer) return; // Ensure only the server checks for players

        // Check the active players list to see if a player has joined
        foreach (GameObject player in PlayerList)
        {
            VRPlayerRig playerRig = player.GetComponent<VRPlayerRig>();
            if (!_currentPlayers.Contains(playerRig))
            {
                Log($"A player joined with id {playerRig.currentPlayerId}");
                _currentPlayers.Add(playerRig);
            }
        }
    }

    private void AddNecessaryScripts()
    {
        if (GrabbableObjects == null || GrabbableObjects.Length == 0)
        {
            GrabbableObjects = GameObject.FindGameObjectsWithTag("Grabbable");

            if (GrabbableObjects.Length == 0)
            {
                LogError("No objects found with the 'Grabbable' tag!");
                return;
            }
        }

        foreach (var grabbable in GrabbableObjects)
        {
            if (grabbable == null)
            {
                LogError("Found a null Grabbable object reference!");
                continue;
            }

            AddComponent(grabbable, typeof(Rigidbody));
            AddComponent(grabbable, typeof(NetworkIdentity));
            AddComponent(grabbable, typeof(NetworkTransformReliable));
            AddComponent(grabbable, typeof(NetworkRigidbodyReliable));

            NetworkRigidbodyReliable networkRb = grabbable.GetComponent<NetworkRigidbodyReliable>();
            if (networkRb != null)
            {
                networkRb.target = grabbable.transform;
                networkRb.enabled = true;
            }
            else
            {
                LogError($"NetworkRigidbodyReliable component missing on {grabbable.name}");
            }
        }
    }
    
    private void AddComponent(GameObject gameObject, Type componentType)
    {
        if (gameObject.GetComponent(componentType) == null)
        {
            gameObject.AddComponent(componentType);
        }
    }
    
    [Server]
    public void AddPlayer(VRPlayerRig player)
    {
        if (!PlayerList.Contains(player.gameObject))
        {
            PlayerList.Add(player.gameObject);
            Log($"Player {player.name} added to playerList");
        }
    }
    
    [Server]
    public void RemovePlayer(VRPlayerRig player)
    {
        if (PlayerList.Contains(player.gameObject))
        {
            PlayerList.Remove(player.gameObject);
            Log($"Player {player.name} removed from playerList");
        }
    }

    private void OnPlayerListChanged(SyncList<VRPlayerRig>.Operation op, int index, VRPlayerRig oldItem, VRPlayerRig newItem)
    {
        Log($"Player list updated: {op.ToString()}");
    }

    /// <summary>
    /// Logger Methods
    /// </summary>
    public void Log(string message)
    {
        Debug.Log(message);
        if (text != null)
        {
            text.text += "\n" + message;
        }
    }

    public void LogError(string message)
    {
        Debug.LogError(message);
        if (text != null)
        {
            text.text += "\n" + message;
        }
    }
}
