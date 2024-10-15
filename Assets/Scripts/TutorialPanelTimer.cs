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
        Time.timeScale = 1f; // Resume the game
        closeButton.onClick.RemoveListener(CloseTutorial); // Remove listener to prevent memory leaks

        // Calculate the off-screen position
        float offScreenPosition = -Screen.height * 2f; // Adjust this value to your liking

        // Animate the panel moving off-screen and then deactivate it
        tutorialPanel.LeanMoveLocalY(offScreenPosition, 2f).setEaseOutExpo().setOnComplete(() =>
        {
            tutorialPanel.SetActive(false);
        });
    }
}

