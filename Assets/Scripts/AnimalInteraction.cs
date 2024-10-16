using System.Collections;
using UnityEngine;

public class AnimalInteraction : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool isDragging = false;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer

    public float matchDistance = 1.0f; // Distance within which the animal is considered matched
    public bool IsMatched { get; private set; } // Property to track if the animal is matched

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
        IsMatched = false;
        initialPosition = transform.position;
    }

    void OnMouseDown()
    {
        isDragging = true;
        PlaySound();
    }

    void OnMouseUp()
    {
        isDragging = false;
        StopSound();
        CheckIfMatched();
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition();
            Vector3 clampedPosition = ClampPosition(newPosition);
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, initialPosition.z);
        }
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        // Define the screen bounds
        float minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x;
        float maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)).x;
        float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y;
        float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y;

        // Clamp the position within the screen bounds
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Convert mouse position to world position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void PlaySound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void StopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void CheckIfMatched()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, matchDistance);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Habitat")) // Directly using the string tag
            {
                // The animal is within matchDistance of a habitat
                IsMatched = true;
                Debug.Log($"{gameObject.name} matched with habitat.");

                // Play LeanTween animation after matching
                PlayMatchAnimation();

                return;
            }
        }

        // No need to handle color change here. `DestroyOnCollision` will take care of it.
    }

    public void PlayMatchAnimation()
    {
        // Disable interaction while the animation is playing
        this.enabled = false;

        // Example: scale up, rotate, then scale down to original size with a small rotation effect
        Vector3 originalScale = transform.localScale;
        float rotationAngle = 30f; // Set the desired rotation angle

        LeanTween.scale(gameObject, originalScale * 1.5f, 0.2f).setEaseOutBack();
        LeanTween.rotateZ(gameObject, rotationAngle, 0.1f).setEaseInOutSine().setLoopPingPong(2).setOnComplete(() =>
        {
            LeanTween.scale(gameObject, originalScale, 0.2f).setEaseInBack().setOnComplete(() =>
            {
                // Reactivate the script or proceed with the match logic after the animation
                GameManager.Instance.AnimalMatched();

                // Destroy the animal after animation completes
                Destroy(gameObject);
            });
        });
    }


}
