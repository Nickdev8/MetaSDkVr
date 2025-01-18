using UnityEngine;
using Mirror;

public class PlayerNetwork : NetworkBehaviour
{
    public Transform sceneCenter; // Assign the center of the scene in the Inspector
    public Transform head; // Assign the XR Rig's head Transform
    public Transform leftHand; // Assign the XR Rig's left hand Transform
    public Transform rightHand; // Assign the XR Rig's right hand Transform

    [SyncVar] public Vector3 headPosition;
    [SyncVar] public Quaternion headRotation;
    [SyncVar] public Vector3 leftHandPosition;
    [SyncVar] public Quaternion leftHandRotation;
    [SyncVar] public Vector3 rightHandPosition;
    [SyncVar] public Quaternion rightHandRotation;

    private void Update()
    {
        if (!isLocalPlayer) return;

        // Update positions and rotations relative to the scene center
        Vector3 relativeHeadPos = sceneCenter.InverseTransformPoint(head.position);
        Quaternion relativeHeadRot = Quaternion.Inverse(sceneCenter.rotation) * head.rotation;

        CmdUpdatePositions(
            relativeHeadPos,
            relativeHeadRot,
            sceneCenter.InverseTransformPoint(leftHand.position),
            Quaternion.Inverse(sceneCenter.rotation) * leftHand.rotation,
            sceneCenter.InverseTransformPoint(rightHand.position),
            Quaternion.Inverse(sceneCenter.rotation) * rightHand.rotation
        );
    }

    [Command]
    private void CmdUpdatePositions(Vector3 hPos, Quaternion hRot, Vector3 lhPos, Quaternion lhRot, Vector3 rhPos, Quaternion rhRot)
    {
        headPosition = hPos;
        headRotation = hRot;
        leftHandPosition = lhPos;
        leftHandRotation = lhRot;
        rightHandPosition = rhPos;
        rightHandRotation = rhRot;
    }
}