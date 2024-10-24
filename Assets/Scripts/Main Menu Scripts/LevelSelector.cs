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
    public GameObject loadingScreen; // This GameObject has the Image component
    public Slider progressBar;
    public Sprite[] levelLoadingImages; // Array of sprites for level-specific loading screens
    private Image loadingScreenImage; // Reference to the Image component on the loading screen

    private void Start()
    {
        // Get the Image component from the loadingScreen GameObject
        loadingScreenImage = loadingScreen.GetComponent<Image>();
        if (loadingScreenImage == null)
        {
            Debug.LogError("No Image component found on loadingScreen GameObject!");
        }

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
        StartCoroutine(LoadLevelAsync(levelName, levelIndex - 1)); // Subtract 1 because array indices start at 0
    }

    IEnumerator LoadLevelAsync(string levelName, int levelIndex)
    {
        // Activate the loading screen
        loadingScreen.SetActive(true);

        // Set the loading screen image based on the level
        if (loadingScreenImage != null && levelIndex >= 0 && levelIndex < levelLoadingImages.Length)
        {
            loadingScreenImage.sprite = levelLoadingImages[levelIndex];
        }
        else
        {
            Debug.LogWarning("No loading image available for level " + (levelIndex + 1));
        }

        // Set the slider's value to 0 at the start
        progressBar.value = 0f;

        // Begin loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        // Prevent the scene from activating immediately when it's loaded
        operation.allowSceneActivation = false;

        float fakeProgress = 0f;  // This will simulate the progress bar if the loading is too fast
        float timer = 0f;         // Timer to ensure a minimum display time

        // While the scene is loading, update the progress bar
        while (!operation.isDone)
        {
            // The actual progress goes from 0 to 0.9, so normalize it
            float actualProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Move fakeProgress gradually towards actualProgress
            fakeProgress = Mathf.MoveTowards(fakeProgress, actualProgress, Time.deltaTime / 7f);  // Adjust speed by changing divisor (higher is slower)

            // Update the progress bar with the fake progress
            progressBar.value = fakeProgress;

            // Increment the timer
            timer += Time.deltaTime;

            // If the scene is fully loaded (operation.progress == 0.9f) and 3 seconds have passed, allow the scene to activate
            if (operation.progress >= 0.9f && timer >= 7f)
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