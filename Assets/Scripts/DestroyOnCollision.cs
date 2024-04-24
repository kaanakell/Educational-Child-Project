using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(gameObject.tag)) // Compare tags of the colliding objects
        {
            Destroy(collision.gameObject);
        }
    }
}

