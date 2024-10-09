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
    [SerializeField] TMP_Text playerQuiz2Text;
    [SerializeField] TMP_Text playerQuiz3Text;
    [SerializeField] TMP_Text playerQuiz4Text;
    [SerializeField] TMP_Text playerQuiz5Text;
    [SerializeField] TMP_Text playerQuiz6Text;
    [SerializeField] TMP_Text playerQuiz7Text;
    [SerializeField] TMP_Text playerQuiz8Text;

    [Header("Text Fields")]
    [SerializeField] GameObject NameUI;
    [SerializeField] GameObject SectionUI;

    private void Start()
    {
        CheckLoginStatus();
    }

    private void CheckLoginStatus()
    {
        if (IsUserLoggedIn())
        {
            mainMenuCanvas.SetActive(true);
            loginCanvas.SetActive(false);
            ReloadProfileData();
        }
    }

    private bool IsUserLoggedIn()
    {
        var authContext = PlayFabSettings.staticPlayer;

        if (authContext != null && !string.IsNullOrEmpty(authContext.ClientSessionTicket))
        {
            return true;
        }

        if (authContext != null && !string.IsNullOrEmpty(authContext.EntityToken))
        {
            return true;
        }

        return false;
    }

    void OnEnable()
    {
        UserProfile.OnProfileDataUpdated.AddListener(ProfileDataUpdated);
        UserProfile.OnQuizzesUpdated.AddListener(QuizzesDataUpdated);
        UserAccountManager.OnSignInSuccess.AddListener(SignIn);
        UserAccountManager.OnUserDataRecieved.AddListener(UserDataReceived);
    }

    void OnDisable()
    {
        UserProfile.OnProfileDataUpdated.RemoveListener(ProfileDataUpdated);
        UserProfile.OnQuizzesUpdated.RemoveListener(QuizzesDataUpdated);
        UserAccountManager.OnSignInSuccess.RemoveListener(SignIn);
        UserAccountManager.OnUserDataRecieved.RemoveListener(UserDataReceived);
    }

    public void ReloadProfileData()
    {
        if (UserAccountManager.Instance == null)
        {
            UserAccountManager.Instance = FindObjectOfType<UserAccountManager>();
        }

        if (UserAccountManager.Instance != null)
        {
            UserAccountManager.Instance.GetUserData("ProfileData");
            UserAccountManager.Instance.GetUserData("QuizzesScores");
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
        var playFabHttpInstance = PlayFab.Internal.PlayFabHttp.instance;
        if (playFabHttpInstance != null)
        {
            Destroy(playFabHttpInstance.gameObject);
            Debug.Log("PlayFabHttp instance destroyed.");
        }
    }

    void UserDataReceived(string key, string value)
    {
        if (key == "ProfileData")
        {
            ProfileData profileData = JsonUtility.FromJson<ProfileData>(value);
            ProfileDataUpdated(profileData);
        }
        else if (key == "QuizzesScores")
        {
            QuizzesScores quizData = JsonUtility.FromJson<QuizzesScores>(value);
            QuizzesDataUpdated(quizData);
        }
    }

    void ProfileDataUpdated(ProfileData profileData)
    {
        if (profileData != null)
        {
            playerNameText.text = "Player Name: " + profileData.playerName;
            playerSectionText.text = "Player Section: " + profileData.Student_Section;
            playerLevelText.text = "Level Completed: " + profileData.level.ToString();

            NameUI.SetActive(string.IsNullOrEmpty(profileData.playerName));
            SectionUI.SetActive(string.IsNullOrEmpty(profileData.Student_Section));
        }
    }

    void QuizzesDataUpdated(QuizzesScores quizData)
    {
        if (quizData != null)
        {
            UpdateQuizText(playerQuiz1Text, "Quiz 1", quizData.QuizNumber1);
            UpdateQuizText(playerQuiz2Text, "Quiz 2", quizData.QuizNumber2);
            UpdateQuizText(playerQuiz3Text, "Quiz 3", quizData.QuizNumber3);
            UpdateQuizText(playerQuiz4Text, "Quiz 4", quizData.QuizNumber4);
            UpdateQuizText(playerQuiz5Text, "Quiz 5", quizData.QuizNumber5);
            UpdateQuizText(playerQuiz6Text, "Quiz 6", quizData.QuizNumber6);
            UpdateQuizText(playerQuiz7Text, "Quiz 7", quizData.QuizNumber7);
            UpdateQuizText(playerQuiz8Text, "Quiz 8", quizData.QuizNumber8);
        }
    }

    private void UpdateQuizText(TMP_Text quizText, string quizName, string score)
    {
        quizText.text = string.IsNullOrEmpty(score) ? $"{quizName}: Not taken" : $"{quizName}: {score}";
    }
}