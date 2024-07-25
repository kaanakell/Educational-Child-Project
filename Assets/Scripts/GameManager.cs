using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject endGamePanel;
    private int totalAnimals;
    private int matchedAnimals;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        endGamePanel.SetActive(false);
        totalAnimals = FindObjectsOfType<AnimalInteraction>().Length;
        matchedAnimals = 0;
    }

    public void AnimalMatched()
    {
        matchedAnimals++;
        CheckEndCondition();
    }

    public void CheckEndCondition()
    {
        if (matchedAnimals >= totalAnimals)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        endGamePanel.SetActive(true);
        // Add any other end game logic here, like stopping the game or showing scores.
    }
}
