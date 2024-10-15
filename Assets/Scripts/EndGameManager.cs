using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;
    public GameObject endGamePanel; // Reference to the end game panel

    private bool isEndGamePanelShown;

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
            // Set the initial position of the end game panel off-screen
            float offScreenPosition = Screen.height + 150f; // Adjust this value as needed
            endGamePanel.transform.localPosition = new Vector3(0, offScreenPosition, 0);

            endGamePanel.SetActive(false); // Ensure the end game panel is initially hidden
        }
        else
        {
            Debug.LogError("End Game Panel is not assigned!");
        }
    }

    public void ShowEndGamePanel()
    {
        if (isEndGamePanelShown) return; // Prevent multiple animations
        isEndGamePanelShown = true;

        if (endGamePanel != null)
        {
            // Activate the panel after a slight delay for smoother transition
            LeanTween.delayedCall(0.1f, () =>
            {
                float offScreenPosition = Screen.height + 150f;
                endGamePanel.transform.localPosition = new Vector3(0, offScreenPosition, 0);
                endGamePanel.SetActive(true);
                endGamePanel.LeanMoveLocalY(0, 0.5f).setEaseInExpo().setIgnoreTimeScale(true);
            });
        }
        else
        {
            Debug.LogError("End Game Panel is not assigned!");
        }
    }

}
