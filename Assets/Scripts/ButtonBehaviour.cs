using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public GameObject settingsMenuUI;

    bool isSettingsPanelActive = false;

    public void LoadScene(string scene_name)
    {
        GameSettings.Instance.AdjustSettingsForLevel(scene_name);
        SceneManager.LoadScene(scene_name);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // Reload the scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("The button is working");
    }

    public void Settings()
    {
        if(isSettingsPanelActive == false){
            settingsMenuUI.SetActive(true);
        } 
    }

    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
    }

}

