using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaceHolderGroup : MonoBehaviour
{
    public LetterPlaceholder[] letterPlaceholders; // Array of letter placeholders in this group
    public Button checkButton; // Reference to the button to check answers
    public AlertUI alertUI;
    public TMP_Text clueTMP; // Reference to the Clue TMP field
    public string correctClueText = "Here is your clue!"; // Text to display when correct
    private bool isAnswerCorrect = false; // Track if the answer is correct

    private void Start()
    {
        // Add a listener to the button to call CheckAnswers when clicked
        if (checkButton != null)
        {
            checkButton.onClick.AddListener(CheckAnswers);
        }
    }

    public void CheckAnswers()
    {
        // Don't allow checking if the answer is already correct
        if (isAnswerCorrect) return;

        bool allCorrect = true;

        // Check each placeholder in the group
        foreach (var placeholder in letterPlaceholders)
        {
            if (!placeholder.HasCorrectLetter())
            {
                allCorrect = false; // If any letter is incorrect, set to false
                break;
            }
        }

        if (allCorrect)
        {
            Debug.Log("All letters are correct!");
            CorrectUIController.Instance.ShowCorrectUI();

            foreach (var placeholder in letterPlaceholders)
            {
                placeholder.SetToUntagged(); // Set all placeholders to "Untagged"
            }

            // Display clue text in Clue TMP
            if (clueTMP != null)
            {
                clueTMP.text = correctClueText;
            }

            isAnswerCorrect = true; // Mark the answer as correct
            checkButton.interactable = false; // Disable the button
        }
        else
        {
            alertUI.ShowAlert("Wrong Answer", "Alert");
            Debug.Log("Some letters are incorrect.");
        }
    }
}
