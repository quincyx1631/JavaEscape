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
    }

    // Method to check if the answer is correct
    void CheckAnswer(int index)
    {
        // If the question has already been answered, do nothing
        if (isAnswered[index]) return;

        // If an item is currently spawning, prevent further clicks
        if (isSpawningItem)
        {
            alertUI.ShowAlert("Wait for the item to spawn.");
            return;
        }

        // If the input matches the correct answer
        if (inputFields[index].text == correctAnswers[index])
        {
            // Enable the corresponding TextMeshPro feedback
            feedbackTexts[index].gameObject.SetActive(true);

            // Disable and hide the input field so the player cannot edit the answer anymore
            inputFields[index].gameObject.SetActive(false);

            // Mark the question as answered
            isAnswered[index] = true;

            // Start spawning the item and disable further input until spawning is done
            StartCoroutine(SpawnItemWithDelay());
        }
        else
        {
            alertUI.ShowAlert("Wrong Answer");
            Debug.Log("Incorrect answer for question " + (index + 1));
        }
    }

    // Coroutine to ensure item spawns and disable further input while spawning
    IEnumerator SpawnItemWithDelay()
    {
        // Set the flag to true to prevent further clicks
        isSpawningItem = true;

        // Call the SpawnRandomItem function from RandomItemSpawner
        itemSpawner.SpawnRandomItem();

        // Wait for a short time (or frame) to ensure item is spawned
        yield return new WaitForSeconds(1f);  // Adjust the delay as needed

        // Set the flag back to false to allow input again
        isSpawningItem = false;
    }
}
