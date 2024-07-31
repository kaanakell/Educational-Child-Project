using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;
    public GameObject endGamePanel; // Reference to the end game panel

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Do not use DontDestroyOnLoad here
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false); // Ensure the end game panel is initially hidden
        }
        else
        {
            Debug.LogError("End Game Panel is not assigned!");
        }
    }

    public void ShowEndGamePanel()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("End Game Panel is not assigned!");
        }
    }
}
