using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] GameObject[] patternImages; // For multiple image questions
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] QuestionSO question;

    void Start()
    {
        SetupQuestion();
    }

    void SetupQuestion()
    {
        // Set the question text
        questionText.text = question.Question;

        // Set the pattern images
        for (int i = 0; i < patternImages.Length; i++)
        {
            if (i < question.QuestionImages.Count)
            {
                patternImages[i].SetActive(true);
                patternImages[i].GetComponent<Image>().sprite = question.QuestionImages[i];
            }
            else
            {
                patternImages[i].SetActive(false);
            }
        }

        // Set the answer buttons text and images
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < question.Answers.Count)
            {
                answerButtons[i].SetActive(true); // Ensure the button is active
                TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                Image buttonImage = answerButtons[i].GetComponentInChildren<Image>();

                if (buttonText != null)
                {
                    buttonText.text = question.Answers[i].answerText;
                    buttonText.gameObject.SetActive(true); // Ensure the text is active
                }

                if (buttonImage != null)
                {
                    if (question.Answers[i].answerImage != null)
                    {
                        buttonImage.sprite = question.Answers[i].answerImage;
                        buttonImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        buttonImage.enabled = false; // Deactivate the Image component itself if no image is provided
                    }
                }
            }
            else
            {
                answerButtons[i].SetActive(false); // Hide extra buttons if not needed
            }
        }
    }
}








