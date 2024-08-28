using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using PlayFab;
using System;

public class UIProfile : MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject loginCanvas;

    [Header("Student Infomations")]
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMP_Text playerSectionText;

    [Header("Levels")]
    [SerializeField] TMP_Text playerLevelText;

    [Header("Quizzes")]
    [SerializeField] TMP_Text playerQuiz1Text;

    [Header("Text Fields")]
    [SerializeField] GameObject NameUI;
    [SerializeField] GameObject SectionUI;

    private void Start()
    {
        CheckLoginStatus();
    }

    private void CheckLoginStatus()
    {
        // Assuming PlayFabClientAPI is used for client-side login in PlayFab
        if (IsUserLoggedIn())
        {
            // User is already logged in, hide the login canvas
            mainMenuCanvas.SetActive(true);
            loginCanvas.SetActive(false);
            ReloadProfileData();
        }
    }

    private bool IsUserLoggedIn()
    {
        // Check if there is a valid session ticket or entity token
        var authContext = PlayFabSettings.staticPlayer; // Reference to the static player object in PlayFabSettings

        if (authContext != null && !string.IsNullOrEmpty(authContext.ClientSessionTicket))
        {
            // The session ticket exists, the user is logged in
            return true;
        }

        // Alternatively, check for an Entity Token if applicable
        if (authContext != null && !string.IsNullOrEmpty(authContext.EntityToken))
        {
            // The entity token exists, the user is logged in
            return true;
        }

        return false;
    }

    void OnEnable()
    {
        // Re-add the listener for ProfileDataUpdated
        UserProfile.OnProfileDataUpdated.AddListener(ProfileDataUpdated);

        UserAccountManager.OnSignInSuccess.AddListener(SignIn);
    }

    void OnDisable()
    {
        // Remove the listener to avoid memory leaks
        UserProfile.OnProfileDataUpdated.RemoveListener(ProfileDataUpdated);

        UserAccountManager.OnSignInSuccess.RemoveListener(SignIn);
    }

    // Function to reload profile data
    public void ReloadProfileData()
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
        loginCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void MainMenuBack()
    {
        mainMenuCanvas.SetActive(true);
        loginCanvas.SetActive(false);
    }

    private void ReloadCurrentScene()
    {
        // Get the current scene name and reload it
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void playerLogout()
    {
        PlayFabClientAPI.ForgetAllCredentials();

        ReloadCurrentScene();

        Debug.Log("Player logged out successfully.");
    }

    private void DestroyPlayFabHttpInstance()
    {
        // Destroy the PlayFabHttp instance
        var playFabHttpInstance = PlayFab.Internal.PlayFabHttp.instance;
        if (playFabHttpInstance != null)
        {
            Destroy(playFabHttpInstance.gameObject);
            Debug.Log("PlayFabHttp instance destroyed.");
        }
    }

    // Callback to update UI elements when profile data is updated
    void ProfileDataUpdated(ProfileData profileData)
    {
        if (profileData != null)
        {
            //User Infomations
            playerNameText.text = "Player Name: " + profileData.playerName;
            playerSectionText.text = "Player Section: " + profileData.Student_Section;

            //Levels
            playerLevelText.text = "Level Completed: " + profileData.level.ToString();

            //Quizzes
            playerQuiz1Text.text = "Quiz 1: " + profileData.QuizScore_1;
        }

        // Check if the player's name is not null
        if (!string.IsNullOrEmpty(profileData.playerName))
        {
            NameUI.SetActive(false);  // Hide the Name UI
        }
        else
        {
            NameUI.SetActive(true);   // Show the Name UI
        }

        // Check if the player's section is not null
        if (!string.IsNullOrEmpty(profileData.Student_Section))
        {
            SectionUI.SetActive(false);  // Hide the Section UI
        }
        else
        {
            SectionUI.SetActive(true);   // Show the Section UI
        }
    }
}