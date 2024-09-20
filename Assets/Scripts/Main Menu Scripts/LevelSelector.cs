using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons; // Array to hold 8 buttons for levels
    public int maxLevel = 8; // Maximum level, set to 8
    private int currentLevel;
    private UIProfile _profileUI;

    public GameObject floatingMessage; // Reference to the floating message GameObject
    public TextMeshProUGUI floatingMessageText; // TextMeshProUGUI component for the floating message
    public float messageDisplayDuration = 3f; // Duration for the floating message

    private void Start()
    {
        // Subscribe to the profile data updated event
        UserProfile.OnProfileDataUpdated.AddListener(UpdateLevelSelection);

        // Initialize the level buttons based on current profile data
        if (UserProfile.Instance != null)
        {
            UpdateLevelSelection(UserProfile.Instance.profileData);
        }
    }

    // Method to update level selection based on the user's profile data
    public void UpdateLevelSelection(ProfileData profileData)
    {
        currentLevel = profileData.level;

        // Ensure Level 1 is always open
        levelButtons[0].interactable = true;

        // Loop through the level buttons and set interactability
        for (int i = 1; i < levelButtons.Length; i++)
        {
            if (i <= currentLevel)
            {
                levelButtons[i].interactable = true;
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    // Public method to be called when a level button is clicked
    public void OnLevelButtonClick(int levelIndex)
    {
        string levelName = "Level " + levelIndex;
        SceneManager.LoadScene(levelName);
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("Main Menu Final");
    }

    private void Update()
    {
        DetectNonInteractableButtonClick();
    }

    // Method to detect clicks on non-interactable buttons
    private void DetectNonInteractableButtonClick()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>(); // List<> needs System.Collections.Generic
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                Button clickedButton = result.gameObject.GetComponent<Button>();

                if (clickedButton != null && !clickedButton.interactable)
                {
                    // Determine the button's index if it's a non-interactable button
                    for (int i = 0; i < levelButtons.Length; i++)
                    {
                        if (levelButtons[i] == clickedButton && i > currentLevel)
                        {
                            ShowFloatingMessage("You need to finish the previous levels first!");
                            break;
                        }
                    }
                }
            }
        }
    }

    // Method to show the floating message
    private void ShowFloatingMessage(string message)
    {
        floatingMessageText.text = message; // Update the TextMeshProUGUI text
        floatingMessage.SetActive(true); // Show the floating message
        StartCoroutine(HideFloatingMessageAfterDelay());
    }

    // Coroutine to hide the floating message after a delay
    private IEnumerator HideFloatingMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDisplayDuration);
        floatingMessage.SetActive(false); // Hide the message after the delay
    }
}
