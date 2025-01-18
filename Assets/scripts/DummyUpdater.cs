using UnityEngine;
using Mirror;

public class DummyUpdater : NetworkBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    private PlayerNetwork targetPlayer;

    public void Initialize(PlayerNetwork player)
    {
        targetPlayer = player;
    }

    private void Update()
    {
        if (targetPlayer == null) return;

        head.position = targetPlayer.sceneCenter.TransformPoint(targetPlayer.headPosition);
        head.rotation = targetPlayer.sceneCenter.rotation * targetPlayer.headRotation;

        leftHand.position = targetPlayer.sceneCenter.TransformPoint(targetPlayer.leftHandPosition);
        leftHand.rotation = targetPlayer.sceneCenter.rotation * targetPlayer.leftHandRotation;

        rightHand.position = targetPlayer.sceneCenter.TransformPoint(targetPlayer.rightHandPosition);
        rightHand.rotation = targetPlayer.sceneCenter.rotation * targetPlayer.rightHandRotation;
    }
}