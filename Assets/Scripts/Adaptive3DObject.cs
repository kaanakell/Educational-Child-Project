using UnityEngine;

public class Adaptive3DObject : MonoBehaviour
{
    public Camera mainCamera; // Drag your main camera here
    private Vector3 originalScale;
    private float aspectRatio;

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale
        AdjustObject(); // Call the method to adjust object position and scale
    }

    void Update()
    {
        // Continuously check if the aspect ratio changes (i.e., screen resize)
        if (aspectRatio != (float)Screen.width / Screen.height)
        {
            AdjustObject();
        }
    }

    void AdjustObject()
    {
        aspectRatio = (float)Screen.width / Screen.height;

        // Adjust scale based on screen aspect ratio or any custom logic
        float scaleFactor = Mathf.Min(Screen.width, Screen.height) / 1000f;
        transform.localScale = originalScale * scaleFactor;

        // Position object within the viewport (0,0 is bottom-left, 1,1 is top-right)
        // For example, placing it in the top-left corner with some padding
        Vector3 viewportPosition = new Vector3(0.1f, 0.9f, mainCamera.nearClipPlane); // Adjust 0.1 and 0.9 to your liking
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPosition);
        transform.position = worldPosition;

        Debug.Log("Adjusted scale and position for screen size.");
    }
}
