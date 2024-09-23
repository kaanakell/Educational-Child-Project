using System.Collections;
using UnityEngine;

public class AnimalInteraction : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool isDragging = false;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer

    public float matchDistance = 1.0f; // Distance within which the animal is considered matched
    public float resetColorDelay = 2.0f; // Time delay to reset color

    public bool IsMatched { get; private set; } // Property to track if the animal is matched
    private Color originalColor; // To store the original sprite color

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
        IsMatched = false;
        initialPosition = transform.position;
        originalColor = spriteRenderer.color; // Store the original color of the sprite
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
            transform.position = new Vector3(newPosition.x, newPosition.y, initialPosition.z);
        }
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
                GameManager.Instance.AnimalMatched();

                spriteRenderer.color = originalColor; // Reset to original color when matched
                this.enabled = false; // Optional: disable further interaction
                return;
            }
        }

        // If no match is found, turn sprite red and reset after a delay
        if (!IsMatched)
        {
            spriteRenderer.color = Color.red; // Turn the sprite red if no match is found
            initialPosition = transform.position; // Update initial position to the new position
            StartCoroutine(ResetColorAfterDelay()); // Reset the color after a delay
        }
    }

    // Coroutine to reset the sprite color after a delay
    private IEnumerator ResetColorAfterDelay()
    {
        yield return new WaitForSeconds(resetColorDelay);
        spriteRenderer.color = originalColor; // Reset to the original color after the delay
    }
}
