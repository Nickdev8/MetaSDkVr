using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils; // Import the namespace for XROrigin

public class CenterSceneOnPlaySpace : MonoBehaviour
{
    [SerializeField]
    private XROrigin xrOrigin;

    [SerializeField]
    private Vector3 desiredWorldForward = Vector3.forward;

    void Start()
    {
        if (xrOrigin == null)
        {
            Debug.LogError("XR Origin is not assigned. Please assign it in the inspector.");
            return;
        }

        CenterPlaySpace();
    }

    public void CenterPlaySpace()
    {
        var xrInputSubsystem = GetXRInputSubsystem();
        if (xrInputSubsystem != null && xrInputSubsystem.TryRecenter())
        {
            Debug.Log("Recentered XR Play Space.");
        }
        else
        {
            Debug.LogWarning("Failed to recenter XR Play Space.");
        }

        AlignToForwardDirection();
    }

    private void AlignToForwardDirection()
    {
        var currentForward = xrOrigin.transform.forward;
        var rotationToAlign = Quaternion.FromToRotation(currentForward, desiredWorldForward);
        xrOrigin.transform.rotation = rotationToAlign * xrOrigin.transform.rotation;
        Debug.Log("Aligned XR Origin forward direction to the desired world forward direction.");
    }

    private XRInputSubsystem GetXRInputSubsystem()
    {
        var subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances(subsystems);

        return subsystems.Count > 0 ? subsystems[0] : null;
    }
}