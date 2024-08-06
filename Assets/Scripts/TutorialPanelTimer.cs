using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelTimer : MonoBehaviour
{
    public GameObject tutorialPanel; // Assign your panel in the Inspector
    public float displayTime = 1f; // Time to display the panel

    void Start()
    {
        StartCoroutine(ShowTutorial());
    }

    IEnumerator ShowTutorial()
{
    Time.timeScale = 0f; // Pause the game
    tutorialPanel.SetActive(true); // Show the panel
    yield return new WaitForSecondsRealtime(displayTime); // Wait for 5 seconds in real time
    tutorialPanel.SetActive(false); // Hide the panel
    Time.timeScale = 1f; // Resume the game
}

}
