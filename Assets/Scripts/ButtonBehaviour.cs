using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    public GameObject settingsMenuUI;
    private bool isSettingsPanelActive = false;

    public GameObject creditPanel;

    public GameObject endGamePanel; 
    public Button pauseButton;

    private int gameRestart;
    private const string GameRestartKey = "GameRestart"; 
    public string gameMenuSceneName = "Game Menu";

    void Start()
    {
        gameRestart = PlayerPrefs.GetInt(GameRestartKey, 0);
    }

    public void TogglePause()
    {
        if (endGamePanel.activeSelf) 
        {
            Debug.Log("Pause menu cannot be opened because the end game panel is active.");
            return;
        }

        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        // Null check for pauseMenuUI
        if (pauseMenuUI == null)
        {
            Debug.LogError("Pause menu UI is not assigned or is missing.");
            return;
        }

        // Null check for endGamePanel
        if (endGamePanel == null)
        {
            Debug.LogError("End game panel is not assigned or is missing.");
            return;
        }

        // Check if the end game panel is active
        if (endGamePanel.activeSelf) 
        {
            Debug.Log("Pause menu cannot be opened because the end game panel is active.");
            return;
        }

        // Proceed to pause the game if all checks pass
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ShowEndGamePanel()
    {
        endGamePanel.SetActive(true);
        Time.timeScale = 0f;  // Pause the game when the end game panel is shown
        DisablePauseButton(); // Disable the pause button when the end game panel is active
    }

    private void DisablePauseButton()
    {
        if (pauseButton != null)
        {
            pauseButton.interactable = false;  // Disable the pause button
        }
    }

    public void LoadScene(string sceneName)
    {
        if (sceneName == gameMenuSceneName)
        {
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
        }
        else
        {
            GameSettings.Instance.AdjustSettingsForLevel(sceneName);
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
        }
    }

    public void RestartGame()
    {
        gameRestart++;
        PlayerPrefs.SetInt(GameRestartKey, gameRestart); 

        if (AdManager.Instance != null)
        {
            AdManager.Instance.LoadAd();

            if (gameRestart >= 3)
            {
                AdManager.Instance.ShowInterstitialAd();
                gameRestart = 0; 
                PlayerPrefs.SetInt(GameRestartKey, gameRestart); 
            }
        }
        else
        {
            Debug.LogError("AdManager instance is not set. Make sure AdManager is properly initialized.");
        }

        // Directly restart the game without coroutine
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
            Debug.Log("The button is working");
        }
        else
        {
            Debug.LogError("GameManager instance is not set. Make sure GameManager is properly initialized.");
        }
    }

    public void OpenSettings()
    {
        if (!isSettingsPanelActive)
        {
            settingsMenuUI.SetActive(true);
            isSettingsPanelActive = true;
        }
    }

    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
        isSettingsPanelActive = false;
    }

    public void ToggleCreditPanel()
    {
        creditPanel.SetActive(!creditPanel.activeSelf);
    }
}
