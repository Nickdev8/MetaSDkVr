using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class CenterSceneOnPlaySpace : MonoBehaviour
{
    public Transform cubeTransform; // The transform to move to the center of the guardian
    public TMP_Text text;
    public GameObject worldContainer; // The parent object for all world objects

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

            if (points.Length >= 4)
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
        // Convert boundary points to local space
        Vector3 point1 = transform.InverseTransformPoint(points[0]);
        Vector3 point2 = transform.InverseTransformPoint(points[1]);
        Vector3 point3 = transform.InverseTransformPoint(points[2]);
        Vector3 point4 = transform.InverseTransformPoint(points[3]);

        // Calculate midpoints and orientation
        Vector3 pointA = MidPoint(point1, point2);
        Vector3 pointB = MidPoint(point3, point4);

        Vector3 between = pointB - pointA;
        float distance = between.magnitude;

        // Calculate center and orientation
        Vector3 centerPosition = pointA + (between / 2.0f);
        Quaternion centerRotation = Quaternion.LookRotation(between);

        // Apply position and rotation to the world container
        if (worldContainer != null)
        {
            worldContainer.transform.position = centerPosition;
            worldContainer.transform.rotation = centerRotation;

            Debug.Log($"World centered at position: {centerPosition}, rotation: {centerRotation.eulerAngles}");
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

    private Vector3 MidPoint(Vector3 a, Vector3 b)
    {
        return (a + b) / 2.0f;
    }
}
