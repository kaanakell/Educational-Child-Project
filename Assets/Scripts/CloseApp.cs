using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseApp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame function called");
        StartCoroutine(QuitCoroutine());
    }

    private IEnumerator QuitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Application.Quit();
    }
}
