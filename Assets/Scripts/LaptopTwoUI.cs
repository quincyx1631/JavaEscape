using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // For InputField

public class LaptopTwoUI : MonoBehaviour
{
    public List<TMP_InputField> inputFields;     // List of input fields on the laptop
    public List<Button> answerButtons;           // Buttons to check the answers
    public List<string> correctAnswers;          // List of correct answers
    public List<TextMeshProUGUI> feedbackTexts;  // List of TextMeshPro for feedback (displayed when correct)

    public RandomItemSpawner itemSpawner;        // Reference to your RandomItemSpawner script
    public AlertUI alertUI;
    public string typingSoundName;               // Name of the sound effect for typing
    public string spawnDropSoundName;            // Name of the sound effect for item spawn/drop

    private bool[] isAnswered;                   // Array to track if each question has been answered
    private bool isSpawningItem = false;         // Flag to track if an item is currently spawning

    void Start()
    {
        // Initialize the isAnswered array to false
        isAnswered = new bool[inputFields.Count];

        // Make sure feedback texts are disabled initially
        foreach (var feedbackText in feedbackTexts)
        {
            feedbackText.gameObject.SetActive(false);
        }

        // Add listeners to the buttons
        for (int i = 0; i < answerButtons.Count; i++)
        {
            int index = i;  // Avoid closure issue in loops
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }

        // Add typing sound listeners to each input field
        foreach (var inputField in inputFields)
        {
            inputField.onValueChanged.AddListener(delegate { PlayTypingSound(); });
        }
    }

    // Method to play the typing sound
    private void PlayTypingSound()
    {
        AudioManager.Instance.Play(typingSoundName);
    }

    // Method to check if the answer is correct
    void CheckAnswer(int index)
    {
        // If the question has already been answered, do nothing
        if (isAnswered[index]) return;

        // If an item is currently spawning, prevent further clicks
        if (isSpawningItem)
        {
            alertUI.ShowAlert("Wait for the item to spawn.","Alert");
            return;
        }

        // If the input matches the correct answer
        if (inputFields[index].text == correctAnswers[index])
        {
            // Set the feedback text to the player's input (correct answer)
            feedbackTexts[index].text = inputFields[index].text;

            // Enable the corresponding TextMeshPro feedback
            feedbackTexts[index].gameObject.SetActive(true);

            // Disable and hide the input field so the player cannot edit the answer anymore
            inputFields[index].gameObject.SetActive(false);

            // Mark the question as answered
            isAnswered[index] = true;

            // Start spawning the item and disable further input until spawning is done
            isSpawningItem = true;

            // Play the spawn/drop sound effect
            AudioManager.Instance.Play(spawnDropSoundName);

            // Start spawning the item and pass a callback for when the item finishes spawning
            itemSpawner.SpawnRandomItem(OnItemSpawned);
        }
        else
        {
            alertUI.ShowAlert("Wrong Answer", "Error");
            Debug.Log("Incorrect answer for question " + (index + 1));
        }
    }

    // Callback for when the item spawning is completed
    void OnItemSpawned()
    {
        // Reset the flag to allow input again
        isSpawningItem = false;
    }
}
