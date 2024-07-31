using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int totalAnimals;
    private int matchedAnimals;
    private bool isGameEnded = false; // Flag to indicate if the game has ended
    public int gamePlayed = 1;

    private bool _muteFxPermanently = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager instance created.");
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Duplicate GameManager instance destroyed.");
        }
    }

    void Start()
    {
        matchedAnimals = 0;
        isGameEnded = false;
        UpdateTotalAnimalsCount();

        StartCoroutine(DisplayBannerWithDelay());
    }

    private IEnumerator DisplayBannerWithDelay()
    {
        yield return new WaitForSeconds(1f);
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }

    public void UpdateTotalAnimalsCount()
    {
        totalAnimals = FindObjectsOfType<AnimalInteraction>().Length;
        Debug.Log($"Total Animals: {totalAnimals}");
    }

    public void AnimalMatched()
    {
        matchedAnimals++;
        Debug.Log($"Matched Animals: {matchedAnimals}");
        CheckEndCondition();
    }

    public void CheckEndCondition()
    {
        if (matchedAnimals >= totalAnimals && !isGameEnded)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (isGameEnded) return; // Ensure EndGame is only called once

        isGameEnded = true; // Set the flag to true to prevent multiple calls
        Debug.Log("End game condition met.");
        EndGameManager.Instance.ShowEndGamePanel(); // Notify EndGameManager to show the end game panel
    }

    public void RestartGame()
    {
        isGameEnded = false;
        matchedAnimals = 0;
        Time.timeScale = 1f; // Reset the time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void MuteSoundEffectPermanently(bool muted)
    {
        _muteFxPermanently = muted;
    }

    public bool IsSoundEffectMutedPermanently() 
    {
        return _muteFxPermanently;
    }
}
