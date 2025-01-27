using UnityEngine;
using System.Collections.Generic;

public class VRPlayerRig : MonoBehaviour
{
    public Transform rHandTransform;
    public Transform lHandTransform;
    public Transform headTransform;
    
    public VRNetworkPlayerScript localVRNetworkPlayerScript;
    
    // [Header("VR Player")]
    // [SerializeField] private int PlayerId;
    // HashSet is an unsorted list/array
    
    public static List<GameObject> ActivePlayers = new List<GameObject>();
    
    protected virtual void Awake()
    {
        ActivePlayers.Add(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        ActivePlayers.Remove(this.gameObject);
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