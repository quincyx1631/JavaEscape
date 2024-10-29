using UnityEngine;
using TMPro;

public class WhiteBoardRiddle : MonoBehaviour
{
    public string correctAnswer;               // The correct answer to the riddle
    public TMP_InputField answerInputField;    // Reference to the TMP input field
    public TMP_Text correctAnswerText;         // Reference to the TMP text field for displaying the correct answer
    public GameObject blackboard;              // Reference to the blackboard GameObject
    public AlertUI alertUI;
    public Animator boxAnimator;               // Reference to the Animator for the box lid

    public string correctAnswerSound;          // Name of the sound to play for correct answer
    public string incorrectAnswerSound;        // Name of the sound to play for incorrect answer
    public string typingSound;                 // Name of the sound to play when typing

    private void Awake()
    {
        answerInputField.onValueChanged.AddListener(PlayTypingSound);
    }

    private void PlayTypingSound(string text)
    {
        if (!string.IsNullOrEmpty(typingSound))
        {
            AudioManager.Instance.Play(typingSound); // Play typing sound using AudioManager
        }
    }

    // Method to check the player's answer
    public void CheckAnswer()
    {
        string playerAnswer = answerInputField.text.Trim(); // Get the player's input

        // Check if the player's answer is correct
        if (playerAnswer.Equals(correctAnswer, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Correct Answer!"); // Log for debugging
            CorrectUIController.Instance.ShowCorrectUI();

            // Display the player's answer in the TMP text field
            correctAnswerText.text = playerAnswer;

            // Untag the blackboard
            if (blackboard != null)
            {
                blackboard.tag = "Untagged"; // Change the tag of the blackboard
                Debug.Log("Blackboard has been untagged."); // Log for debugging
            }

            // Set the trigger for the box animation
            if (boxAnimator != null)
            {
                boxAnimator.SetTrigger("OpenLid");
                Debug.Log("Box lid is opening.");
            }

            // Play the correct answer sound
            if (!string.IsNullOrEmpty(correctAnswerSound))
            {
                AudioManager.Instance.Play(correctAnswerSound);
            }
        }
        else
        {
            // Show alert and log for incorrect answer
            alertUI.ShowAlert("Incorrect Answer. Try again!!");
            Debug.Log("Incorrect Answer. Try again!");

            // Play the incorrect answer sound
            if (!string.IsNullOrEmpty(incorrectAnswerSound))
            {
                AudioManager.Instance.Play(incorrectAnswerSound);
            }
        }

        // Clear the input field after checking
        answerInputField.text = string.Empty;
    }
}
