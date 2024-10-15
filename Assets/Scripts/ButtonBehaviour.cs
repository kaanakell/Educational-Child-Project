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

    public void Pause()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogError("Pause menu UI is not assigned or is missing.");
            return;
        }

        if (endGamePanel.activeSelf)
        {
            Debug.Log("Pause menu cannot be opened because the end game panel is active.");
            return;
        }

        // Activate pause menu first
        pauseMenuUI.SetActive(true);

        // Calculate the off-screen position
        float offScreenPosition = Screen.height;

        // Set the initial position of the pause menu off-screen
        pauseMenuUI.transform.localPosition = new Vector3(0, offScreenPosition * 1.25f, 0);

        // Animate the pause menu sliding down from the top
        pauseMenuUI.LeanMoveLocalY(0, 0.5f).setEaseInExpo().setIgnoreTimeScale(true);


        Time.timeScale = 0f;
        isPaused = true;
    }


    public void Resume()
    {
        // Calculate the off-screen position
        float offScreenPosition = Screen.height;

        // Animate the pause menu sliding up to the top
        pauseMenuUI.LeanMoveLocalY(offScreenPosition, 0.5f).setEaseInExpo().setOnComplete(() =>
        {
            pauseMenuUI.SetActive(false);
        });

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ShowEndGamePanel()
    {
        // Calculate the off-screen position
        float offScreenPosition = Screen.height + 150f;  // You can adjust this value to push the panel further off-screen

        // Set the initial position of the end game panel off-screen
        endGamePanel.transform.localPosition = new Vector3(0, offScreenPosition, 0);

        // Activate the panel AFTER setting its position off-screen
        endGamePanel.SetActive(true);

        // Animate the end game panel sliding down from the top
        endGamePanel.LeanMoveLocalY(0, 0.5f).setEaseInExpo().setIgnoreTimeScale(true);

        // Pause the game when the end game panel is shown
        Time.timeScale = 0f;

        // Disable the pause button when the end game panel is active
        DisablePauseButton();
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
            // Calculate the off-screen position
            float offScreenPosition = Screen.height;

            // Set the initial position of the settings menu
            settingsMenuUI.transform.localPosition = new Vector3(0, offScreenPosition, 0);

            // Animate the settings menu sliding down from the below
            settingsMenuUI.LeanMoveLocalY(0, 0.5f).setEaseOutExpo();

            settingsMenuUI.SetActive(true);
            isSettingsPanelActive = true;
        }
    }

    public void CloseSettings()
    {
        if (isSettingsPanelActive)
        {
            // Calculate the off-screen position
            float offScreenPosition = Screen.height;

            // Animate the settings menu sliding up to the below
            settingsMenuUI.LeanMoveLocalY(offScreenPosition, 0.5f).setEaseInExpo().setOnComplete(() =>
            {
                settingsMenuUI.SetActive(false);
                isSettingsPanelActive = false;
            });
        }
    }

    public void ToggleCreditPanel()
    {
        if (creditPanel != null)
        {
            if (creditPanel.activeSelf)
            {
                // Animate hiding the credit panel
                creditPanel.LeanMoveLocalX(-Screen.width, 0.5f).setEaseInExpo().setIgnoreTimeScale(true).setOnComplete(() =>
                {
                    creditPanel.SetActive(false); // Disable the panel after the animation
                });
            }
            else
            {
                // Set the initial position of the credit panel off-screen (to the left)
                creditPanel.transform.localPosition = new Vector3(-Screen.width * 1.25f, 0, 0);
                creditPanel.SetActive(true); // Activate the panel before animating

                // Animate showing the credit panel
                creditPanel.LeanMoveLocalX(0, 0.5f).setEaseInExpo().setIgnoreTimeScale(true);
            }
        }
        else
        {
            Debug.LogError("Credit Panel is not assigned!");
        }
    }

}
