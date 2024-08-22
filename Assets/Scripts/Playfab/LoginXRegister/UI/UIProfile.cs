using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIProfile : MonoBehaviour
{
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMP_Text playerLevelText;
    [SerializeField] TMP_Text playerScoreText;
    [SerializeField] TMP_Text playerQuiz1Text;

    void OnEnable()
    {
        // Re-add the listener for ProfileDataUpdated
        UserProfile.OnProfileDataUpdated.AddListener(ProfileDataUpdated);

        // Reload profile data whenever the scene is loaded or reloaded
        ReloadProfileData();
    }

    void OnDisable()
    {
        // Remove the listener to avoid memory leaks
        UserProfile.OnProfileDataUpdated.RemoveListener(ProfileDataUpdated);
    }

    // Function to reload profile data
    void ReloadProfileData()
    {
        if (UserAccountManager.Instance == null)
        {
            // Attempt to find the UserAccountManager in the scene
            UserAccountManager.Instance = FindObjectOfType<UserAccountManager>();
        }

        if (UserAccountManager.Instance != null)
        {
            UserAccountManager.Instance.GetUserData("ProfileData");
        }
        else
        {
            Debug.LogError("UserAccountManager instance is still not found.");
        }
    }

    // Callback to update UI elements when profile data is updated
    void ProfileDataUpdated(ProfileData profileData)
    {
        if (profileData != null)
        {
            playerNameText.text = "Player Name: " + profileData.playerName;
            playerLevelText.text = "Level Completed: " + profileData.level.ToString();
            playerScoreText.text = UserProfile.Instance.score.ToString();
            playerQuiz1Text.text = "Quiz 1: " + profileData.QuizScore_1.ToString();
        }
    }
}