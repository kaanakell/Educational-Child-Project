using UnityEngine;

public class Adaptive3DObjectScale : MonoBehaviour
{
    private Vector3 originalScale;
    private float aspectRatio;

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale
        AdjustScale(); // Call the method to adjust scale initially
    }

    void Update()
    {
        // If the aspect ratio changes, adjust the scale again
        if (aspectRatio != (float)Screen.width / Screen.height)
        {
            AdjustScale();
        }
    }

    void AdjustScale()
    {
        aspectRatio = (float)Screen.width / Screen.height;
        
        // Adjust the scale based on aspect ratio (custom logic here)
        transform.localScale = originalScale * aspectRatio;
        
        Debug.Log("Scale adjusted based on aspect ratio: " + aspectRatio);
    }
}

