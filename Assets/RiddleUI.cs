using UnityEngine;
using TMPro; // For using TextMeshPro

public class RiddleUI : MonoBehaviour
{
    public TMP_InputField riddleInputField;   // Reference to the input field for the riddle answer
    public string correctAnswer;              // The correct answer for the riddle
    public GameObject riddleScreen;           // The Riddle Screen to disable
    public GameObject riddleUI;               // The entire Riddle UI to hide
    public GameObject clueScreen;             // The Clue Screen to enable

    // Method to check if the answer is correct
    public void CheckAnswer()
    {
        if (riddleInputField.text.Equals(correctAnswer, System.StringComparison.OrdinalIgnoreCase))
        {
            // Disable and hide the Riddle UI and Riddle Screen
            riddleScreen.SetActive(false);
            riddleUI.SetActive(false); // Hides the entire Riddle UI
            clueScreen.SetActive(true); // Enable the Clue Screen
            Debug.Log("Correct answer! Riddle solved. Riddle UI and screen hidden.");
        }
        else
        {
            Debug.Log("Incorrect answer. Try again.");
        }
    }
}
