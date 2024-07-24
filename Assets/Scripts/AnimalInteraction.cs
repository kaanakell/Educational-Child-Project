using UnityEngine;

public class AnimalInteraction : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool isDragging = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        initialPosition = transform.position;
        PlaySound();
    }

    void OnMouseUp()
    {
        isDragging = false;
        StopSound();
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
}
