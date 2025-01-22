using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRPlayerRig : MonoBehaviour
{
    public Transform rHandTransform;
    public Transform lHandTransform;
    public Transform headTransform;
    
    public VRNetworkPlayerScript localVRNetworkPlayerScript;

    private void Update()
    {
        if (localVRNetworkPlayerScript)
        {
            localVRNetworkPlayerScript.headTransform.position = this.headTransform.position;
            localVRNetworkPlayerScript.headTransform.rotation = this.headTransform.rotation;
            localVRNetworkPlayerScript.rHandTransform.position = this.rHandTransform.position;
            localVRNetworkPlayerScript.rHandTransform.rotation = this.rHandTransform.rotation;
            localVRNetworkPlayerScript.lHandTransform.position = this.lHandTransform.position;
            localVRNetworkPlayerScript.lHandTransform.rotation = this.lHandTransform.rotation;
        }
    }
}