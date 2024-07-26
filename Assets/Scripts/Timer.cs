using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject EndGamePanel; // Ensure this is assigned in the Inspector

    private bool isTimerRunning = false;
    private float delayTimer = 3.5f; // Delay before starting the timer
    public float elapsedTime = 0f;

    void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
        else if (!isTimerRunning)
        {
            isTimerRunning = true;
        }

        if (isTimerRunning)
        {
            ElapsedTime();
        }
    }

    private void ElapsedTime()
    {
        if (!IsEndGamePanelActive()) // Check if the end game panel is not active
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    public void ResumeTimer()
    {
        isTimerRunning = true;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    private bool IsEndGamePanelActive()
    {
        // Check if the end game panel is active
        return EndGamePanel != null && EndGamePanel.activeSelf;
    }
}
