using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;

    // Maintain a local list to track already-added players
    private List<GameObject> _currentPlayers = new List<GameObject>();

    private void Update()
    {
        // Check the active players list
        foreach (GameObject player in VRPlayerRig.ActivePlayers)
        {
            // If the player is not already in the local list, add them to the target group
            if (!_currentPlayers.Contains(player))
            {
                NewPlayerJoined(player);
            }
        }

        // Remove players from the target group if they are no longer active
        RemoveInactivePlayers();
    }

    void NewPlayerJoined(GameObject newPlayer)
    {
        // Add the new player to the target group and update the local list
        targetGroup.AddMember(newPlayer.transform, 1, 1);
        _currentPlayers.Add(newPlayer);
        Debug.Log($"New player added to the target group: {newPlayer.name}");
    }

    void RemoveInactivePlayers()
    {
        // Check for players in the local list that are no longer active
        for (int i = _currentPlayers.Count - 1; i >= 0; i--)
        {
            GameObject player = _currentPlayers[i];
            if (!VRPlayerRig.ActivePlayers.Contains(player))
            {
                // Remove the player from the target group and the local list
                targetGroup.RemoveMember(player.transform);
                _currentPlayers.RemoveAt(i);
                Debug.Log($"Player removed from the target group: {player.name}");
            }
        }
    }
}