using UnityEngine;

public class AnimalInteraction : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool isDragging = false;
    private AudioSource audioSource;

    public float matchDistance = 1.0f; // Distance within which the animal is considered matched

    public bool IsMatched { get; private set; } // Property to track if the animal is matched

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
                GameManager.Instance.AnimalMatched();
                // Optional: disable further interaction
                this.enabled = false;
                return;
            }
        }

        // If no match is found, keep the animal at its current position
        if (!IsMatched)
        {
            initialPosition = transform.position; // Update initial position to the new position
        }
    }
}
