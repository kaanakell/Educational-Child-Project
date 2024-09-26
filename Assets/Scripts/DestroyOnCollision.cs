using System.Collections;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the same tag as the habitat
        if (collision.collider.CompareTag(gameObject.tag))
        {
            // Correct match: Destroy the animal game object
            Destroy(collision.gameObject);
            Debug.Log($"{collision.gameObject.name} correctly matched with {gameObject.name} habitat.");
        }
        else
        {
            // Incorrect match: Change animal color to red and log the result
            SpriteRenderer animalSprite = collision.gameObject.GetComponent<SpriteRenderer>();
            if (animalSprite != null)
            {
                animalSprite.color = Color.red; // Turn the sprite red
                Debug.Log($"{collision.gameObject.name} incorrectly matched with {gameObject.name} habitat.");
                
                // Optionally, add a reset color after a delay or other logic to reset the position
                StartCoroutine(ResetColorAfterDelay(animalSprite));
            }
        }
    }

    // Coroutine to reset the sprite color after a delay (if needed)
    private IEnumerator ResetColorAfterDelay(SpriteRenderer animalSprite)
    {
        yield return new WaitForSeconds(0.5f); // Adjust delay as needed
        animalSprite.color = Color.white; // Reset to original color (assuming white is default)
    }
}
