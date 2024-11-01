using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaceHolderGroup : MonoBehaviour
{
    [System.Serializable]
    public class PlaceholderGroupData
    {
        public string groupName; // Identifier for the group (for debugging or display)
        public LetterPlaceholder[] letterPlaceholders; // Array of placeholders in this group
        public Button checkButton; // Button to check this group's answers
        public TMP_Text clueTMP; // Clue text for this group
        public string correctClueText = "Here is your clue!"; // Clue text to display if correct
        public GameObject[] additionalUIElements; // UI elements to activate if correct
        public bool isAnswerCorrect = false; // Track if answer is correct
        public GameObject phoneGameObject; // Reference to the specific phone for this group
    }

    public PlaceholderGroupData[] placeholderGroups; // Array of all groups
    public AlertUI alertUI;
    public string messengerNotif = "Correct";

    public GameObject laptopGameObject;

    private void Start()
    {
        foreach (var group in placeholderGroups)
        {
            if (group.checkButton != null)
            {
                // Explicitly capture the group reference to ensure the correct one is checked
                PlaceholderGroupData currentGroup = group;
                group.checkButton.onClick.RemoveAllListeners(); // Remove any existing listeners to avoid duplicates
                group.checkButton.onClick.AddListener(() => CheckAnswers(currentGroup));
                Debug.Log($"Button for group {currentGroup.groupName} is set up to check only its own placeholders.");
            }
        }
    }

    private void CheckAnswers(PlaceholderGroupData group)
    {
        // Ensure only placeholders within the clicked button's group are checked
        Debug.Log($"Button clicked for group {group.groupName}. Checking answers for this group only.");

        // Only proceed if this group hasn’t already been marked correct
        if (group.isAnswerCorrect)
        {
            Debug.Log($"Group {group.groupName} has already been marked as correct. Skipping check.");
            return;
        }

        bool allCorrect = true;

        // Check only the placeholders in this specific group
        foreach (var placeholder in group.letterPlaceholders)
        {
            if (!placeholder.HasCorrectLetter())
            {
                allCorrect = false; // Stop checking if any letter is incorrect
                break;
            }
        }

        if (allCorrect)
        {
            AudioManager.Instance.Play(messengerNotif);
            Debug.Log($"All letters are correct for group {group.groupName}!");

            CorrectUIController.Instance.ShowCorrectUI();

            foreach (var placeholder in group.letterPlaceholders)
            {
                placeholder.SetToUntagged();
            }

            if (group.clueTMP != null)
            {
                group.clueTMP.text = group.correctClueText;
            }

            foreach (var uiElement in group.additionalUIElements)
            {
                uiElement.SetActive(true);
            }

            group.isAnswerCorrect = true; // Mark this group as correct
            group.checkButton.interactable = false; // Disable the check button for this group

            // Mark the phone associated with this group as collected
            if (group.phoneGameObject != null)
            {
                CollectionManager.Instance.MarkAsCollected(group.phoneGameObject.GetComponent<Interactables>());
                Debug.Log($"Marked {group.phoneGameObject.name} as collected for group {group.groupName}.");
            }
            else
            {
                Debug.LogError($"Phone GameObject reference is missing for group {group.groupName}.");
            }
        }
        else
        {
            alertUI.ShowAlert("Wrong Answer", "Alert");
            Debug.Log($"Some letters are incorrect for group {group.groupName}.");
        }

        // Check if all groups are correct after checking this group
        CheckAllGroupsAreCorrect();
    }

    private void CheckAllGroupsAreCorrect()
    {
        bool allGroupsCorrect = true;

        foreach (var group in placeholderGroups)
        {
            if (!group.isAnswerCorrect)
            {
                allGroupsCorrect = false;
                break; // No need to check further if one group is incorrect
            }
        }

        // If all groups are correct, change the tag of the laptop to "Interactables"
        if (allGroupsCorrect)
        {
            if (laptopGameObject != null)
            {
                laptopGameObject.tag = "Interactables"; // Change tag to Interactables
                Debug.Log("All groups are correct. Laptop tagged as Interactables.");
            }
            else
            {
                Debug.LogError("Laptop GameObject reference is missing.");
            }
        }
    }
}
