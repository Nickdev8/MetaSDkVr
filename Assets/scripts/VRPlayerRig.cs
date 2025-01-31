using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class VRPlayerRig : MonoBehaviour
{
    public Transform rHandTransform;
    public Transform lHandTransform;
    public Transform headTransform;
    
    public VRNetworkPlayerScript localVRNetworkPlayerScript;
    
    public int currentPlayerId;
    public GameManager gameManager;
    
    protected virtual void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.AddPlayer(this);
        currentPlayerId = gameManager.PlayerList.IndexOf(this.gameObject);
        gameManager.Log($"added {this.gameObject.name} to PlayerList with ID {currentPlayerId}");
    }

    protected virtual void OnDestroy()
    {
        gameManager.RemovePlayer(this);
        gameManager.Log($"removed {this.gameObject.name} to PlayerList with ID {currentPlayerId}");
        currentPlayerId = -1;
    }
    
    private void Update()
    {
        if (!localVRNetworkPlayerScript)
            return;
        
        localVRNetworkPlayerScript.headTransform.position = this.headTransform.position;
        localVRNetworkPlayerScript.headTransform.rotation = this.headTransform.rotation;
        localVRNetworkPlayerScript.rHandTransform.position = this.rHandTransform.position;
        localVRNetworkPlayerScript.rHandTransform.rotation = this.rHandTransform.rotation;
        localVRNetworkPlayerScript.lHandTransform.position = this.lHandTransform.position;
        localVRNetworkPlayerScript.lHandTransform.rotation = this.lHandTransform.rotation;
        
    }
}