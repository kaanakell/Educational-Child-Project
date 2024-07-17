using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] private string question = "Enter new question text here";
    [SerializeField] private List<Sprite> questionImages; // Multiple images for pattern quiz

    [System.Serializable]
    public class Answer
    {
        [TextArea(1, 3)]
        public string answerText; // Text for the answer
        public Sprite answerImage; // Image for the answer
    }

    [SerializeField] private List<Answer> answers = new List<Answer>(); // List of possible answers
    [SerializeField] private int correctAnswerIndex; // Index of the correct answer in the answers list

    // Properties to access the fields
    public string Question => question;
    public List<Sprite> QuestionImages => questionImages;
    public List<Answer> Answers => answers;
    public int CorrectAnswerIndex => correctAnswerIndex;
}


