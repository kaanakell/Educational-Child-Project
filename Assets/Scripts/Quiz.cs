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
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();

    private QuestionSO currentQuestion;

    void Start()
    {
        SetupQuestion();
    }

    public void SetupQuestion()
    {
        // Set the question text
        questionText.text = currentQuestion.Question;

        // Set the pattern images
        for (int i = 0; i < patternImages.Length; i++)
        {
            if (i < currentQuestion.QuestionImages.Count)
            {
                patternImages[i].SetActive(true);
                patternImages[i].GetComponent<Image>().sprite = currentQuestion.QuestionImages[i];
            }
            else
            {
                patternImages[i].SetActive(false);
            }
        }

        // Set the answer buttons text and images
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.Answers.Count)
            {
                answerButtons[i].SetActive(true); // Ensure the button is active
                TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                Image buttonImage = answerButtons[i].GetComponent<Image>();

                if (buttonText != null)
                {
                    buttonText.text = currentQuestion.Answers[i].answerText;
                    buttonText.gameObject.SetActive(true); // Ensure the text is active
                }

                if (buttonImage != null)
                {
                    if (currentQuestion.Answers[i].answerImage != null)
                    {
                        buttonImage.sprite = currentQuestion.Answers[i].answerImage;
                        buttonImage.enabled = true; // Enable the Image component
                    }
                    else
                    {
                        buttonImage.enabled = false; // Disable the Image component if no image is provided
                    }
                }

                // Clear previous click listeners to prevent multiple calls
                answerButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();

                // Add click listener
                int answerIndex = i; // Capture the current index for the closure
                answerButtons[i].GetComponent<Button>().onClick.AddListener(() => CheckAnswer(answerIndex));
            }
            else
            {
                answerButtons[i].SetActive(false); // Hide extra buttons if not needed
            }
        }
        SetButtonState(true);
    }

    void GetNextQuestion()
    {
        if (questions.Count == 0)
        {
            GetRandomQuestion();
            Debug.Log("No more questions available.");
            return; // No more questions to display
        }
        //SetupQuestion();
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        questions.RemoveAt(index); // Remove the question from the list to avoid repeats
    }

    public void CheckAnswer(int index)
    {
        bool isCorrect = index == currentQuestion.CorrectAnswerIndex;

        // Log the result
        Debug.Log(isCorrect ? "Correct answer!" : "Wrong answer!");

        // Change button color
        Color color = isCorrect ? Color.green : Color.red;
        answerButtons[index].GetComponent<Image>().color = color;

        // Lock buttons
        SetButtonState(false);

        // If the answer is correct, proceed to the next question
        if (isCorrect)
        {
            Invoke("GetNextQuestion", 1f); // Delay before loading next question
        }
        else
        {
            // If the answer is wrong, ask the same question again
            Invoke("SetupQuestion", 1f); // Delay before re-asking the same question
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;

            if (state)
            {
                // Reset button color to default when re-enabling
                answerButtons[i].GetComponent<Image>().color = Color.white;
            }
        }
    }
}
