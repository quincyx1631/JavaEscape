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

    void Start()
    {
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
        // If the input matches the correct answer
        if (inputFields[index].text == correctAnswers[index])
        {
            // Enable the corresponding TextMeshPro feedback
            feedbackTexts[index].gameObject.SetActive(true);

            // Disable and hide the input field so the player cannot edit the answer anymore
            inputFields[index].gameObject.SetActive(false);

            // Call the SpawnRandomItem function from RandomItemSpawner
            itemSpawner.SpawnRandomItem();
        }
        else
        {
            Debug.Log("Incorrect answer for question " + (index + 1));
        }
    }
}
