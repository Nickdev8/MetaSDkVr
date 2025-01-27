using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class CenterSceneOnPlaySpace : MonoBehaviour
{
    public Transform cubeTransform; // The transform to move to the center of the guardian
    public TMP_Text text;
    public GameObject worldContainer; // The parent object for all world objects (e.g., OVRCameraRig)

    private OVRBoundary boundary;

    private void Start()
    {
        // Ensure boundary system is initialized
        if (OVRManager.boundary != null)
        {
            boundary = new OVRBoundary();
        }

        // Add tracking change event to handle recentering
        OVRManager.TrackingAcquired += UpdateCenter;

        UpdateCenter();
    }

    private void OnDestroy()
    {
        // Clean up event subscription
        OVRManager.TrackingAcquired -= UpdateCenter;
    }

    private void UpdateCenter()
    {
        if (boundary != null && OVRManager.boundary.GetConfigured())
        {
            Vector3[] points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

            if (points != null && points.Length > 0)
            {
                CenterWorld(points);
            }
            else
            {
                Debug.LogWarning("Insufficient boundary points to calculate center.");
            }
        }
        else
        {
            Debug.LogWarning("Boundary not configured or OVRManager not available.");
        }
    }

    private void CenterWorld(Vector3[] points)
    {
        // Calculate the average position of all boundary points to find the center
        Vector3 centerPosition = Vector3.zero;
        foreach (Vector3 point in points)
        {
            centerPosition += point;
        }
        centerPosition /= points.Length;

        // Convert the center position to world space
        centerPosition = transform.TransformPoint(centerPosition);

        // Apply the calculated center position to the world container
        if (worldContainer != null)
        {
            Vector3 offset = worldContainer.transform.position - centerPosition;

            worldContainer.transform.position -= offset;

            Debug.Log($"World centered at position: {centerPosition}");
        }

        // Optionally move the cubeTransform to the center
        if (cubeTransform != null)
        {
            cubeTransform.position = centerPosition;
            if (text != null)
            {
                text.text = centerPosition.ToString();
            }
        }
    }
}
