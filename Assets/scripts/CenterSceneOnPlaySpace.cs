using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class CenterSceneOnPlaySpace : MonoBehaviour
{
    [Header("Transforms to Adjust")]
    public GameObject worldContainer;   // Parent object for all world objects (e.g., OVRCameraRig)
    public Transform cubeTransform;     // Optional: To visualize the center of the Guardian
    public Transform planeTransform;    // The Plane to be scaled to fit the Guardian's bounds
    
    [Header("Optional Debug UI")]
    public TMP_Text text;

    private OVRBoundary boundary;

    private void Start()
    {
        // Initialize boundary system
        if (OVRManager.boundary != null)
        {
            boundary = new OVRBoundary();
        }

        // Subscribe to tracking events in case Guardian changes or re-centers
        OVRManager.TrackingAcquired += UpdateCenter;

        // Initial setup
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
                CenterAndRotateWorld(points);
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

    private void CenterAndRotateWorld(Vector3[] points)
    {
        // 1. Calculate the average position of all boundary points to find the "center"
        Vector3 localCenter = Vector3.zero;
        foreach (Vector3 point in points)
        {
            localCenter += point;
        }
        localCenter /= points.Length;

        // Convert from local to world space
        Vector3 worldCenter = transform.TransformPoint(localCenter);

        // 2. Compute the primary forward direction from boundary shape (for stable alignment)
        Vector3 forwardDirection = ComputeGuardianForwardDirection(points);

        // Generate the desired rotation
        Quaternion targetRotation = Quaternion.LookRotation(forwardDirection, Vector3.up);

        // 3. Reposition & rotate the entire world container
        if (worldContainer != null)
        {
            // Apply rotation first (so position offset is correct)
            worldContainer.transform.rotation = targetRotation;
            // Move to the Guardian's center
            worldContainer.transform.position = worldCenter;

            Debug.Log($"World repositioned to {worldCenter} with rotation {targetRotation.eulerAngles}");
        }

        // 4. (Optional) Move the visual "cube" to show the center
        if (cubeTransform != null)
        {
            cubeTransform.position = worldCenter;
            if (text != null) 
                text.text = $"Center: {worldCenter}";
        }

        // 5. Scale the plane to fit the Guardian
        if (planeTransform != null)
        {
            ScalePlaneToGuardian(points, worldCenter, targetRotation);
        }
    }

    /// <summary>
    /// Computes a forward direction by finding the longest edge in the boundary and aligning to it.
    /// </summary>
    private Vector3 ComputeGuardianForwardDirection(Vector3[] points)
    {
        // Start with a default forward
        float maxDistance = 0f;
        Vector3 bestDirection = Vector3.forward;

        // Compare every pair of points to find the longest boundary edge
        for (int i = 0; i < points.Length; i++)
        {
            for (int j = i + 1; j < points.Length; j++)
            {
                float distance = Vector3.Distance(points[i], points[j]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    bestDirection = (points[j] - points[i]).normalized;
                }
            }
        }

        // Force direction to be purely horizontal
        bestDirection.y = 0;
        bestDirection.Normalize();

        return bestDirection;
    }

    /// <summary>
    /// Scales and positions the plane so it fits the Guardian bounds 
    /// via an axis-aligned bounding box (in XZ plane).
    /// </summary>
    private void ScalePlaneToGuardian(Vector3[] points, Vector3 worldCenter, Quaternion targetRotation)
    {
        // 1. Transform boundary points to world space (if they are in local).
        //    But note: `points` are typically returned in local tracking space.
        //    So let's keep them local, then compute bounding box in local space,
        //    and position/scale plane in that same space for consistency.
        
        float minX = float.PositiveInfinity;
        float maxX = float.NegativeInfinity;
        float minZ = float.PositiveInfinity;
        float maxZ = float.NegativeInfinity;

        foreach (Vector3 point in points)
        {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.z < minZ) minZ = point.z;
            if (point.z > maxZ) maxZ = point.z;
        }

        float width  = (maxX - minX);
        float length = (maxZ - minZ);

        // 2. Because Unity's default Plane is 10×10, we need to adjust by /10 
        //    if you want the plane to match boundary size exactly.
        //    If your plane is a 1×1 custom mesh, skip the division by 10.
        
        float scaleX = width  / 10f;
        float scaleZ = length / 10f;

        // 3. Apply the rotation & position
        planeTransform.rotation = targetRotation;
        planeTransform.position = worldCenter;

        // 4. Scale it so that it fits inside the bounding box
        planeTransform.localScale = new Vector3(scaleX, 1f, scaleZ);

        Debug.Log($"Plane scaled to X:{scaleX} Z:{scaleZ} and positioned at {worldCenter}");
    }
}
