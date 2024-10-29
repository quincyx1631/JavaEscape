using UnityEngine;
using TMPro; // For using TextMeshPro

public class RiddleUI : MonoBehaviour
{
    public TMP_InputField riddleInputField;   // Reference to the input field for the riddle answer
    public string correctAnswer;              // The correct answer for the riddle
    public GameObject riddleScreen;           // The Riddle Screen to disable
    public GameObject riddleUI;               // The entire Riddle UI to hide
    public GameObject clueScreen;             // The Clue Screen to enable
    public AlertUI alertUI;

    public CollectionUI collectionUI;         // Reference to CollectionUI to mark items as collected
    public string itemToCollect;              // The name of the item to collect upon solving the riddle

    public TMP_Text passwordText;             // TextMeshPro reference for displaying the password
    public string nextTaskPassword;           // The password for the next task (set in Inspector)

    public string typingSoundName;            // Name of the typing sound in the AudioManager

    private void Awake()
    {
        riddleInputField.onValueChanged.AddListener(PlayTypingSound);
    }

    private void PlayTypingSound(string text)
    {
        if (!string.IsNullOrEmpty(typingSoundName))
        {
            AudioManager.Instance.Play(typingSoundName); // Use AudioManager to play typing sound
        }
    }

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
            CorrectUIController.Instance.ShowCorrectUI();

            // Mark the corresponding item as collected in CollectionUI
            collectionUI.OnItemCollected(itemToCollect);  // Call the method to mark item as collected

            // Set the password for the next task on the Clue Screen
            passwordText.text = nextTaskPassword;  // Display the password on the Clue Screen
        }
        else
        {
            Debug.Log("Incorrect answer. Try again.");
            alertUI.ShowAlert("Incorrect Answer");
        }
    }
}
