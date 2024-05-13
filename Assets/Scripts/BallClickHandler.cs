using UnityEngine;
using TMPro;

public class BallClickHandler : MonoBehaviour
{
    public TextMeshProUGUI inputField; // Reference to the input field in Canvas
    public char letter; // The representative letter of the ball

    void OnMouseDown()
    {
        // Check if the input field reference is valid
        if (inputField != null)
        {
            // Update the text of the input field with the representative letter
            inputField.text += letter;
        }
    }
}

