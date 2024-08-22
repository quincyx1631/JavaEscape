using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;

public class UIProfile : MonoBehaviour
{
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMP_Text playerLevelText;
    [SerializeField] TMP_Text playerScoreText;
    [SerializeField] TMP_Text playerQuiz1Text;

    private bool isSignInSuccessful = false;

    private void OnEnable()
    {
        // Add listeners
        UserProfile.OnProfileDataUpdated.AddListener(ProfileDataUpdated);
        UserAccountManager.OnSignInSuccess.AddListener(OnSignIn);

        // Start coroutine to check sign-in status
        StartCoroutine(CheckSignInStatus());
    }

    private void OnDisable()
    {
        // Remove listeners
        UserProfile.OnProfileDataUpdated.RemoveListener(ProfileDataUpdated);
        UserAccountManager.OnSignInSuccess.RemoveListener(OnSignIn);

        // Stop coroutine when disabled
        StopCoroutine(CheckSignInStatus());
    }

    private IEnumerator CheckSignInStatus()
    {
        // Wait until sign-in is successful
        while (!isSignInSuccessful)
        {
            yield return null; // Wait for the next frame
        }

        // Once sign-in is successful, reload profile data
        ReloadProfileData();
    }

    private void OnSignIn()
    {
        isSignInSuccessful = true;
    }

    private void ReloadProfileData()
    {
        if (UserAccountManager.Instance != null)
        {
            UserAccountManager.Instance.GetUserData("ProfileData");
        }
        else
        {
            Debug.LogError("UserAccountManager instance is not found.");
        }
    }

    private void ProfileDataUpdated(ProfileData profileData)
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
