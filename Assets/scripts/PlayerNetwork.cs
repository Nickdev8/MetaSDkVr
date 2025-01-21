using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    private NetworkVariable<Vector3> headPosition = new NetworkVariable<Vector3>();
    private NetworkVariable<Quaternion> headRotation = new NetworkVariable<Quaternion>();
    private NetworkVariable<Vector3> leftHandPosition = new NetworkVariable<Vector3>();
    private NetworkVariable<Quaternion> leftHandRotation = new NetworkVariable<Quaternion>();
    private NetworkVariable<Vector3> rightHandPosition = new NetworkVariable<Vector3>();
    private NetworkVariable<Quaternion> rightHandRotation = new NetworkVariable<Quaternion>();

    void Update()
    {
        if (IsOwner)
        {
            // Update network variables with local VR transform data
            headPosition.Value = head.position;
            headRotation.Value = head.rotation;
            leftHandPosition.Value = leftHand.position;
            leftHandRotation.Value = leftHand.rotation;
            rightHandPosition.Value = rightHand.position;
            rightHandRotation.Value = rightHand.rotation;
        }
        else
        {
            // Update local dummy transforms with synced data
            head.position = headPosition.Value;
            head.rotation = headRotation.Value;
            leftHand.position = leftHandPosition.Value;
            leftHand.rotation = leftHandRotation.Value;
            rightHand.position = rightHandPosition.Value;
            rightHand.rotation = rightHandRotation.Value;
        }
    }
}
