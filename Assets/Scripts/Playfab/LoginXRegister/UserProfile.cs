using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UserProfile : MonoBehaviour
{
    public static UserProfile Instance;

    public static UnityEvent<ProfileData> OnProfileDataUpdated = new UnityEvent<ProfileData>();

    public static UnityEvent<QuizzesScores> OnQuizzesUpdated = new UnityEvent<QuizzesScores>();

    [SerializeField] public ProfileData profileData;

    [SerializeField] public QuizzesScores quizData;

    private int maxLevel = 8;

    private void Awake()
    {
        Instance = this;

        if (profileData == null)
        {
            profileData = new ProfileData();

            //Levels
            profileData.level = 0;

            //Level 1
            profileData.Level_1_Timer = "";
            profileData.Level_1_Score = "";

            //Level 2
            profileData.Level_2_Timer = "";
            profileData.Level_2_Score = "";

            //Level 3
            profileData.Level_3_Timer = "";
            profileData.Level_3_Score = "";

            //Level 4
            profileData.Level_4_Timer = "";
            profileData.Level_4_Score = "";

            //Level 5
            profileData.Level_5_Timer = "";
            profileData.Level_5_Score = "";

            //Level 6
            profileData.Level_6_Timer = "";
            profileData.Level_6_Score = "";

            //Level 7
            profileData.Level_7_Timer = "";
            profileData.Level_7_Score = "";

            //Level 8
            profileData.Level_8_Timer = "";
            profileData.Level_8_Score = "";
        }

        if (quizData == null)
        {
            quizData = new QuizzesScores();

            quizData.QuizNumber1 = "";
            quizData.QuizNumber2 = "";
            quizData.QuizNumber3 = "";
            quizData.QuizNumber4 = "";
            quizData.QuizNumber5 = "";
            quizData.QuizNumber6 = "";
            quizData.QuizNumber7 = "";
            quizData.QuizNumber8 = "";
        }
    }

    void OnEnable()
    {
        UserAccountManager.OnSignInSuccess.AddListener(SignIn);

        UserAccountManager.OnUserDataRecieved.AddListener(UserDataRecieved);

        UserAccountManager.OnUserDataRecieved.AddListener(QuizDataRecieved);
    }

    void OnDisable()
    {
        UserAccountManager.OnSignInSuccess.RemoveListener(SignIn);

        UserAccountManager.OnUserDataRecieved.RemoveListener(UserDataRecieved);

        UserAccountManager.OnUserDataRecieved.RemoveListener(QuizDataRecieved);
    }

    void SignIn()
    {
        GetUserData();
        GetQuizData();
    }

    [ContextMenu("Get Profile Data")]
    void GetUserData()
    {
        UserAccountManager.Instance.GetUserData("ProfileData");
    }

    void UserDataRecieved(string key, string value)
    {
        if (key == "ProfileData")
        {
            if (!string.IsNullOrEmpty(value))
            {
                profileData = JsonUtility.FromJson<ProfileData>(value);
            }
            else
            {
                // If the value is empty, ensure profileData is initialized
                profileData = new ProfileData();
            }
            OnProfileDataUpdated.Invoke(profileData);
        }
    }

    [ContextMenu("Set Profile Data")]
    void SetUserData(UnityAction OnSucess = null)
    {
        UserAccountManager.Instance.SetUserData("ProfileData", JsonUtility.ToJson(profileData), OnSucess);
    }

    void QuizDataRecieved(string key, string value)
    {
        if (key == "QuizzesScores")
        {
            if (!string.IsNullOrEmpty(value))
            {
                quizData = JsonUtility.FromJson<QuizzesScores>(value);
            }
            else
            {
                quizData = new QuizzesScores();
            }
            OnQuizzesUpdated.Invoke(quizData);
        }
    }

    [ContextMenu("Get Quiz Scores")]
    void GetQuizData()
    {
        UserAccountManager.Instance.GetUserData("QuizzesScores");
    }

    [ContextMenu("Set Quiz Data")]
    void SetQuizData(UnityAction OnSucess = null)
    {
        UserAccountManager.Instance.SetQuizData("QuizzesScores", JsonUtility.ToJson(quizData), OnSucess);
    }

    public void AddLevel(int levelIndex)
    {
        if (profileData.level < maxLevel)
        {
            Door timerLevel = FindAnyObjectByType<Door>();
            FinishUI levelScore = FindAnyObjectByType<FinishUI>();

            // Convert finalElapsedTime (which is in seconds) to TimeSpan
            TimeSpan timeSpan = TimeSpan.FromSeconds(timerLevel.finalElapsedTime);

            // Format the TimeSpan into a string with minutes and seconds
            string timerLevelSave = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            // Use reflection or dynamic properties to set timer and score for the correct level
            switch (levelIndex)
            {
                case 1:
                    profileData.Level_1_Timer = timerLevelSave;
                    profileData.Level_1_Score = levelScore.GetFinalScore().ToString();
                    break;
                case 2:
                    profileData.Level_2_Timer = timerLevelSave;
                    profileData.Level_2_Score = levelScore.GetFinalScore().ToString();
                    break;
                case 3:
                    profileData.Level_3_Timer = timerLevelSave;
                    profileData.Level_3_Score = levelScore.GetFinalScore().ToString();
                    break;
                case 4:
                    profileData.Level_4_Timer = timerLevelSave;
                    profileData.Level_4_Score = levelScore.GetFinalScore().ToString();
                    break;
                case 5:
                    profileData.Level_5_Timer = timerLevelSave;
                    profileData.Level_5_Score = levelScore.GetFinalScore().ToString();
                    break;
                case 6:
                    profileData.Level_6_Timer = timerLevelSave;
                    profileData.Level_6_Score = levelScore.GetFinalScore().ToString();
                    break;
                case 7:
                    profileData.Level_7_Timer = timerLevelSave;
                    profileData.Level_7_Score = levelScore.GetFinalScore().ToString();
                    break;
                case 8:
                    profileData.Level_8_Timer = timerLevelSave;
                    profileData.Level_8_Score = levelScore.GetFinalScore().ToString();
                    break;
                default:
                    Debug.LogWarning("Invalid level index.");
                    break;
            }
            if (levelIndex > profileData.level)
            {
                profileData.level += 1;
                Debug.Log("Sucessfully Added Level to Database");
                SetUserData();
                GetUserData();
            }
            else
            {
                Debug.Log("You already completed this level");
            }
        }
        else
        {
            Debug.Log("Maximum level reached");
        }
    }

    //Quiz 1
    public void SetQuizScore()
    {
        QuizManager quizManager = FindObjectOfType<QuizManager>();
        if (quizManager != null)
        {
            // Check if the quiz has already been completed
            if (string.IsNullOrEmpty(quizData.QuizNumber1))
            {
                quizData.QuizNumber1 = quizManager.score.ToString() + "/10";
                SetQuizData(GetQuizData);
            }
            else
            {
                Debug.Log("You already completed Quiz 1");
            }
        }
    }

    //Quiz 2
    public void SetQuiz2Score()
    {
        QuizManager2 quiz2Manager = FindObjectOfType<QuizManager2>();
        if (quiz2Manager != null)
        {
            // Check if the quiz has already been completed
            if (string.IsNullOrEmpty(quizData.QuizNumber2))
            {
                quizData.QuizNumber2 = quiz2Manager.score.ToString() + "/10";
                SetQuizData(GetQuizData);
            }
            else
            {
                Debug.Log("You already completed Quiz 2");
            }
        }
    }

    //Quiz 3
    public void SetQuiz3Score()
    {
        QuizManager3 quiz3Manager = FindObjectOfType<QuizManager3>();
        if (quiz3Manager != null)
        {
            // Check if the quiz has already been completed
            if (string.IsNullOrEmpty(quizData.QuizNumber3))
            {
                quizData.QuizNumber3 = quiz3Manager.score.ToString() + "/10";
                SetQuizData(GetQuizData);
            }
            else
            {
                Debug.Log("You already completed Quiz 3");
            }
        }
    }

    //Quiz 4
    public void SetQuiz4Score()
    {
        QuizManager4 quiz4Manager = FindObjectOfType<QuizManager4>();
        if (quiz4Manager != null)
        {
            // Check if the quiz has already been completed
            if (string.IsNullOrEmpty(quizData.QuizNumber4))
            {
                quizData.QuizNumber4 = quiz4Manager.score.ToString() + "/10";
                SetQuizData(GetQuizData);
            }
            else
            {
                Debug.Log("You already completed Quiz 4");
            }
        }
    }

    //Quiz 5
    public void SetQuiz5Score()
    {
        QuizManager5 quiz5Manager = FindObjectOfType<QuizManager5>();
        if (quiz5Manager != null)
        {
            // Check if the quiz has already been completed
            if (string.IsNullOrEmpty(quizData.QuizNumber5))
            {
                quizData.QuizNumber5 = quiz5Manager.score.ToString() + "/10";
                SetQuizData(GetQuizData);
            }
            else
            {
                Debug.Log("You already completed Quiz 5");
            }
        }
    }

    //Quiz 6
    public void SetQuiz6Score()
    {
        QuizManager6 quiz6Manager = FindObjectOfType<QuizManager6>();
        if (quiz6Manager != null)
        {
            // Check if the quiz has already been completed
            if (string.IsNullOrEmpty(quizData.QuizNumber6))
            {
                quizData.QuizNumber6 = quiz6Manager.score.ToString() + "/10";
                SetQuizData(GetQuizData);
            }
            else
            {
                Debug.Log("You already completed Quiz 6");
            }
        }
    }

    //Quiz 7
    public void SetQuiz7Score()
    {
        QuizManager7 quiz7Manager = FindObjectOfType<QuizManager7>();
        if (quiz7Manager != null)
        {
            // Check if the quiz has already been completed
            if (string.IsNullOrEmpty(quizData.QuizNumber7))
            {
                quizData.QuizNumber7 = quiz7Manager.score.ToString() + "/10";
                SetQuizData(GetQuizData);
            }
            else
            {
                Debug.Log("You already completed Quiz 7");
            }
        }
    }

    //Quiz 8
    public void SetQuiz8Score()
    {
        QuizManager8 quiz8Manager = FindObjectOfType<QuizManager8>();
        if (quiz8Manager != null)
        {
            // Check if the quiz has already been completed
            if (string.IsNullOrEmpty(quizData.QuizNumber8))
            {
                quizData.QuizNumber8 = quiz8Manager.score.ToString() + "/10";
                SetQuizData(GetQuizData);
            }
            else
            {
                Debug.Log("You already completed Quiz 8");
            }
        }
    }

    public void SetPlayerName(string displayName)
    {
        // Check if the display name is not null or empty
        if (!string.IsNullOrEmpty(displayName))
        {
            profileData.playerName = displayName;

            // Try to set the display name first
            UserAccountManager.Instance.SetDisplayName(displayName, displayNameSuccess =>
            {
                if (displayNameSuccess)
                {
                    // If setting the display name succeeds, set the user data
                    SetUserData(GetUserData);
                    Debug.Log("Player name and user data set successfully.");
                }
                else
                {
                    Debug.LogError("Failed to set display name. User data was not set.");
                }
            });
        }
        else
        {
            Debug.LogWarning("Display name is empty. Please enter a valid name.");
        }
    }


    public void SetPlayerSection(string playerSection)
    {
        // Check if the player section is not null or empty
        if (!string.IsNullOrEmpty(playerSection))
        {
            profileData.Student_Section = playerSection;
            SetUserData(GetUserData);
        }
        else
        {
            Debug.LogWarning("Player section is empty. Please enter a valid section.");
        }
    }

}

[System.Serializable]
public class ProfileData
{
    //Player Data
    public string playerName;
    public string Student_Section;

    //Level & Time completion
    public int level;
    //Level 
    public string Level_1_Timer;
    public string Level_1_Score;

    //Level 2
    public string Level_2_Timer;
    public string Level_2_Score;

    //Level 3
    public string Level_3_Timer;
    public string Level_3_Score;

    //Level 4
    public string Level_4_Timer;
    public string Level_4_Score;

    //Level 4
    public string Level_5_Timer;
    public string Level_5_Score;

    //Level 4
    public string Level_6_Timer;
    public string Level_6_Score;

    //Level 4
    public string Level_7_Timer;
    public string Level_7_Score;

    //Level 4
    public string Level_8_Timer;
    public string Level_8_Score;
}

[System.Serializable]
public class QuizzesScores
{
    //Quizzes Scores
    public string QuizNumber1;
    public string QuizNumber2;
    public string QuizNumber3;
    public string QuizNumber4;
    public string QuizNumber5;
    public string QuizNumber6;
    public string QuizNumber7;
    public string QuizNumber8;
}