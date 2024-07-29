using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public float waitTime = 30f; // Time to wait before transitioning
    public string nextSceneName; // Name of the next scene to load

    void Start()
    {
        // Start the coroutine that waits for the specified time and then transitions to the next scene
        StartCoroutine(WaitAndLoadScene());
    }

    IEnumerator WaitAndLoadScene()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}

