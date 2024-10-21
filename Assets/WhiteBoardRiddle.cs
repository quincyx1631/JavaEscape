using UnityEngine;
using TMPro; // Make sure you have TextMeshPro namespace
using System.Collections; // Needed for coroutines

public class WhiteBoardRiddle : MonoBehaviour
{
    public string correctAnswer;               // The correct answer to the riddle
    public TMP_InputField answerInputField;    // Reference to the TMP input field
    public TMP_Text correctAnswerText;         // Reference to the TMP text field for displaying the correct answer
    public GameObject blackboard;              // Reference to the blackboard GameObject
    public AlertUI alertUI;
    public GameObject boxLid;                  // Reference to the lid of the box
    public float lidRemoveDelay = 1.5f;        // Time in seconds before the box lid disappears

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

            // Start coroutine to remove the box lid after a delay
            if (boxLid != null)
            {
                StartCoroutine(RemoveBoxLidWithDelay());
            }
        }
        else
        {
            alertUI.ShowAlert("Incorrect Answer. Try again!!");
            Debug.Log("Incorrect Answer. Try again!"); // Log for incorrect answer
        }

        // Clear the input field after checking
        answerInputField.text = string.Empty;
    }

    // Coroutine to remove the box lid after a delay
    private IEnumerator RemoveBoxLidWithDelay()
    {
        yield return new WaitForSeconds(lidRemoveDelay); // Wait for the specified delay
        boxLid.SetActive(false); // Deactivate the box lid to simulate removing it
        Debug.Log("Box lid has been removed after delay."); // Log for debugging
    }
}
