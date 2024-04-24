using UnityEngine;

public class RowTracking : MonoBehaviour
{
    private Transform mainCameraTransform;
    private Vector3 initialOffset; // Offset between row's initial position and camera's initial position

    void Start()
    {
        // Get reference to the main camera's transform
        mainCameraTransform = Camera.main.transform;

        // Calculate the initial offset between the row's initial position and the camera's initial position
        initialOffset = transform.position - mainCameraTransform.position;
    }

    void Update()
    {
        // Calculate the target position for the row based on the camera's movement
        Vector3 targetPosition = mainCameraTransform.position + initialOffset;

        // Set the row's position to the target position
        transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
    }

    // Method to update the row's position based on the positions of the individual objects in the row
    public void UpdateRowPosition(Vector3 newPosition)
    {
        // Calculate the average position of all objects in the row
        Vector3 averagePosition = Vector3.zero;
        int objectCount = 0;

        foreach (Transform child in transform)
        {
            averagePosition += child.position;
            objectCount++;
        }

        if (objectCount > 0)
        {
            averagePosition /= objectCount;
            transform.position = new Vector3(averagePosition.x, transform.position.y, transform.position.z);
        }
    }
}


