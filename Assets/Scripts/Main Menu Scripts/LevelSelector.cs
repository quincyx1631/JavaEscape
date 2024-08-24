using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons; // Array to hold 8 buttons for levels
    public int maxLevel = 8; // Maximum level, set to 8
    private int currentLevel;
    private UIProfile _profileUI;

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
}
