using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class CenterSceneOnPlaySpace : MonoBehaviour
{
    public Transform cubeTransform; // The transform to move to the center of the guardian
    public TMP_Text text;

    void Start()
    {
        CenterSceneOnGuardian();
    }

    private void CenterSceneOnGuardian()
    {
        // Get the active XRInputSubsystem
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);

        if (subsystems.Count == 0)
        {
            Debug.LogWarning("No XRInputSubsystems found.");
            return;
        }

        XRInputSubsystem xrSubsystem = subsystems[0]; // Assuming the first one is valid

        // Retrieve the boundary points
        List<Vector3> boundaryPoints = new List<Vector3>();
        if (xrSubsystem.TryGetBoundaryPoints(boundaryPoints) && boundaryPoints.Count > 0)
        {
            // Calculate the center of the boundary
            Vector3 center = Vector3.zero;
            foreach (Vector3 point in boundaryPoints)
            {
                center += point;
            }
            center /= boundaryPoints.Count;

            // Log boundary points (optional)
            Debug.Log("Boundary Points: ");
            foreach (var point in boundaryPoints)
            {
                Debug.Log(point);
            }

            // Move the cube to the center of the boundary
            if (cubeTransform != null)
            {
                cubeTransform.position = center;
                text.text = cubeTransform.position.ToString();
                Debug.Log($"Cube moved to center of play space: {center}");
            }
        }
        else
        {
            Debug.LogWarning("Unable to retrieve boundary points or no boundary points available.");
        }
    }
}