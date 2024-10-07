using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public GameObject tutorialPanel; // Assign your panel in the Inspector
    public Button closeButton; // Assign your close button in the Inspector

    void Start()
    {
        // Ensure the tutorial panel is shown when the game starts
        ShowTutorial();
    }

    public void ShowTutorial()
    {
        Time.timeScale = 0f; // Pause the game
        tutorialPanel.SetActive(true); // Show the panel
        closeButton.onClick.AddListener(CloseTutorial); // Add listener for the close button
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false); // Hide the panel
        Time.timeScale = 1f; // Resume the game
        closeButton.onClick.RemoveListener(CloseTutorial); // Remove listener to prevent memory leaks
    }
}

