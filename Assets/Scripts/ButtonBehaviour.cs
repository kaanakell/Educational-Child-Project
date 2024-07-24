using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    public void LoadScene(string scene_name)
    {
        GameSettings.Instance.AdjustSettingsForLevel(scene_name);
        SceneManager.LoadScene(scene_name);
    }

    public void RestartGame()
    {
        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("The button is working");
    }
}

