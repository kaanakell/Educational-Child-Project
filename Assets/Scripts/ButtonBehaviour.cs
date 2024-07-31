using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    // Pause Menu UI
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    // Settings Menu UI
    public GameObject settingsMenuUI;
    private bool isSettingsPanelActive = false;

    // Button Sounds
    public AudioClip buttonSoundEffect;
    private AudioSource audioSource;

    private int gameRestart;
    private const string GameRestartKey = "GameRestart"; // Key for PlayerPrefs

    // Scene Names
    public string gameMenuSceneName = "Game Menu"; // Set this to your actual game menu scene name

    void Start()
    {
        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Load the game restart count from PlayerPrefs
        gameRestart = PlayerPrefs.GetInt(GameRestartKey, 0);
        Debug.Log($"Loaded Game Restart Count: {gameRestart}");
    }

    public void TogglePause()
    {
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
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadScene(string sceneName)
    {
        if (sceneName == gameMenuSceneName)
        {
            // Directly load the Game Menu scene without sound effect
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
        }
        else
        {
            StartCoroutine(PlaySoundAndLoadScene(sceneName));
        }
    }

    public void RestartGame()
    {
        gameRestart++;
        Debug.Log($"Game Restart Count: {gameRestart}");
        PlayerPrefs.SetInt(GameRestartKey, gameRestart); // Save the count to PlayerPrefs

        AdsManager.Instance.bannerAds.ShowBannerAd();

        if (gameRestart >= 3)
        {
            Debug.Log("Attempting to show interstitial ad...");
            AdsManager.Instance.interstitialAds.ShowInterstitialAd();
            AdsManager.Instance.bannerAds.HideBannerAd();
            gameRestart = 0; // Reset count after showing ad
            PlayerPrefs.SetInt(GameRestartKey, gameRestart); // Save the reset count to PlayerPrefs
        }

        StartCoroutine(PlaySoundAndRestartGame());
    }

    public void OpenSettings()
    {
        PlaySound();
        if (!isSettingsPanelActive)
        {
            settingsMenuUI.SetActive(true);
            isSettingsPanelActive = true;
        }
    }

    public void CloseSettings()
    {
        PlaySound();
        settingsMenuUI.SetActive(false);
        isSettingsPanelActive = false;
    }

    private void PlaySound()
    {
        if (buttonSoundEffect != null)
        {
            audioSource.PlayOneShot(buttonSoundEffect);
        }
        else
        {
            Debug.LogWarning("Button sound effect is not assigned.");
        }
    }

    private IEnumerator PlaySoundAndLoadScene(string sceneName)
    {
        PlaySound();
        // Wait for the sound effect to finish
        yield return new WaitForSeconds(buttonSoundEffect.length);
        GameSettings.Instance.AdjustSettingsForLevel(sceneName); // Adjust settings if necessary
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    private IEnumerator PlaySoundAndRestartGame()
    {
        PlaySound();
        // Wait for the sound effect to finish
        yield return new WaitForSeconds(buttonSoundEffect.length);
        // Reload the scene using GameManager's RestartGame method
        GameManager.Instance.RestartGame();
        Debug.Log("The button is working");
    }
}
