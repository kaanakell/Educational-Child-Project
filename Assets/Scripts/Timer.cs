using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private GameObject TimerObject;
    [SerializeField] TextMeshProUGUI timerText;
    private GameObject EndGamePanel;

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

    public void ElapsedTime()
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

    private bool IsEndGamePanelActive()
    {
        // Check if the end game panel is active
        return EndGamePanel != null && EndGamePanel.activeSelf;
    }
}