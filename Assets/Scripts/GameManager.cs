using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int totalAnimals; // This should reflect the total number of animals across all groups
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
        AdManager.Instance.LoadAd();
    }

    void Start()
    {
        matchedAnimals = 0;
        isGameEnded = false;
        // Set total animals based on the array in AnimalInstantiation
        totalAnimals = AnimalInstantiation.Instance.GetTotalAnimalCount();
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
            if (gamePlayed % 3 == 0)
            {
                AdManager.Instance.ShowInterstitialAd();
            }
        }
    }

    public void EndGame()
    {
        if (isGameEnded) return;

        isGameEnded = true;
        Debug.Log("End game condition met.");
        EndGameManager.Instance.ShowEndGamePanel();
    }

    public void RestartGame()
    {
        isGameEnded = false;
        matchedAnimals = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

