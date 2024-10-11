using UnityEngine;

public class Adaptive3DObjectPosition : MonoBehaviour
{
    public Camera mainCamera; // Drag your main camera here

    void Start()
    {
        AdjustPosition();
    }

    void Update()
    {
        // Call this in Update if you want the position to adjust when screen size changes
        AdjustPosition();
    }

    void AdjustPosition()
    {
        // Example: Placing the object in the top-left corner of the screen
        Vector3 screenPosition = new Vector3(0, Screen.height, mainCamera.nearClipPlane);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        
        transform.position = worldPosition;

        Debug.Log("Position adjusted to screen edge.");
    }
}
