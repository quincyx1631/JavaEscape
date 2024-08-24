using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIProfile : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMP_Text playerLevelText;
    [SerializeField] TMP_Text playerScoreText;
    [SerializeField] TMP_Text playerQuiz1Text;


    void OnEnable()
    {
        // Re-add the listener for ProfileDataUpdated
        UserProfile.OnProfileDataUpdated.AddListener(ProfileDataUpdated);

        UserAccountManager.OnSignInSuccess.AddListener(SignIn);

        /*        // Reload profile data whenever the scene is loaded or reloaded
                ReloadProfileData();*/
    }

    void OnDisable()
    {
        // Remove the listener to avoid memory leaks
        UserProfile.OnProfileDataUpdated.RemoveListener(ProfileDataUpdated);

        UserAccountManager.OnSignInSuccess.RemoveListener(SignIn);
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

    public void SignIn()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
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