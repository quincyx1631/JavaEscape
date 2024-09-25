using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    [Header("Level Selector Buttons")]
    public Button[] levelButtons; // Array to hold 8 buttons for levels
    public int maxLevel = 8; // Maximum level, set to 8
    private int currentLevel;
    private UIProfile _profileUI;

    [Header("Warning Messages When Button is Disabled")]
    public GameObject floatingMessage; // Reference to the floating message GameObject
    public TextMeshProUGUI floatingMessageText; // TextMeshProUGUI component for the floating message
    public float messageDisplayDuration = 3f; // Duration for the floating message

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public Slider progressBar;

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
        StartCoroutine(LoadLevelAsync(levelName));
    }

    // Coroutine to load the level asynchronously with a progress bar and a 3-second delay
    IEnumerator LoadLevelAsync(string levelName)
    {
        // Activate the loading screen
        loadingScreen.SetActive(true);

        // Set the slider's value to 0 at the start
        progressBar.value = 0f;

        // Begin loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        // Prevent the scene from activating immediately when it's loaded
        operation.allowSceneActivation = false;

        float timer = 0f;  // Timer to keep track of loading time

        // While the scene is loading, update the progress bar
        while (!operation.isDone)
        {
            // Normalize operation.progress from 0 to 0.9 into 0 to 1
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;

            // Increment the timer
            timer += Time.deltaTime;

            // If the scene is fully loaded (operation.progress == 0.9f) and 3 seconds have passed, allow the scene to activate
            if (operation.progress >= 0.9f && timer >= 3f)
            {
                // Activate the scene after the delay
                operation.allowSceneActivation = true;
            }

            yield return null; // Wait for the next frame before continuing the loop
        }

        // Once loading is complete, deactivate the loading screen
        loadingScreen.SetActive(false);
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
