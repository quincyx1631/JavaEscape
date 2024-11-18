using TMPro;
using UnityEngine;

public class CrosswordInputField : MonoBehaviour
{
    public TMP_InputField inputField; // The TMP_InputField component
    public string correctLetter; // The correct letter for this input field
    private bool isLocked = false;

    private string soundName = "highlight"; // Private sound name, set to a default sound (you can change this to your preferred sound)

    private void Start()
    {
        inputField.onValueChanged.AddListener(OnInputChanged);
        inputField.text = ""; // Start with an empty input field
    }

    // Listen for input changes
    private void OnInputChanged(string enteredText)
    {
        if (isLocked) return;

        // Limit input to 1 letter and capitalize it
        if (enteredText.Length > 1)
        {
            enteredText = enteredText.Substring(0, 1); // Limit to one letter
        }
        inputField.text = enteredText.ToUpper(); // Update the input field to uppercase

        // Play the sound effect when a letter is entered
        if (!string.IsNullOrEmpty(enteredText)) // Only play sound if there is a letter entered
        {
            AudioManager.Instance.Play(soundName); // Play the sound
        }
    }

    // Check if the current letter is correct
    public bool IsCorrect()
    {
        return inputField.text == correctLetter;
    }

    // Lock the input field when the letter is correct and when the group is completed
    public void LockInput()
    {
        isLocked = true;
        inputField.interactable = false; // Disable the input field to prevent further input
    }
}
