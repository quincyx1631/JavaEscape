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

    [SerializeField] public ProfileData profileData;


    private void Awake()
    {
        Instance = this;

        if (profileData == null)
        {
            profileData = new ProfileData();

            //Levels
            profileData.level = 1; // Default level value
            profileData.Level_1_Timer = "";

            //User Informations
            //profileData.playerName = ""; // Default name
            //profileData.Student_Section = "";

            //Quizzes
            profileData.QuizScore_1 = "";
            profileData.QuizScore_2 = "";
            profileData.QuizScore_3 = "";
            profileData.QuizScore_4 = "";
            profileData.QuizScore_5 = "";
            profileData.QuizScore_6 = "";
            profileData.QuizScore_7 = "";
            profileData.QuizScore_8 = "";
        }
    }

    void OnEnable()
    {
        UserAccountManager.OnSignInSuccess.AddListener(SignIn);

        UserAccountManager.OnUserDataRecieved.AddListener(UserDataRecieved);
    }

    void OnDisable()
    {
        UserAccountManager.OnSignInSuccess.RemoveListener(SignIn);

        UserAccountManager.OnUserDataRecieved.RemoveListener(UserDataRecieved);
    }

    void SignIn()
    {
        GetUserData();
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

    //REFERENCE FOR LEVEL PROGRESSION NI JAVEN
    public void AddLevel()
    {
        const int maxLevel = 8;

        if (profileData.level < maxLevel)
        {
            profileData.level += 1;
            Door timerLevel = FindAnyObjectByType<Door>();

            // Convert finalElapsedTime (which is in seconds) to TimeSpan
            TimeSpan timeSpan = TimeSpan.FromSeconds(timerLevel.finalElapsedTime);

            // Format the TimeSpan into a string with minutes and seconds
            string timerLevelSave = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            // Save the formatted time string into profileData
            profileData.Level_1_Timer = timerLevelSave;
            SetUserData();
            GetUserData();
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
            if (string.IsNullOrEmpty(profileData.QuizScore_1))
            {
                profileData.QuizScore_1 = quizManager.score.ToString() + "/10";
                SetUserData(GetUserData); 
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
            if (string.IsNullOrEmpty(profileData.QuizScore_2))
            {
                profileData.QuizScore_2 = quiz2Manager.score.ToString() + "/10";
                SetUserData(GetUserData); 
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
            if (string.IsNullOrEmpty(profileData.QuizScore_3))
            {
                profileData.QuizScore_3 = quiz3Manager.score.ToString() + "/10";
                SetUserData(GetUserData); 
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
            if (string.IsNullOrEmpty(profileData.QuizScore_4))
            {
                profileData.QuizScore_4 = quiz4Manager.score.ToString() + "/10";
                SetUserData(GetUserData); 
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
            if (string.IsNullOrEmpty(profileData.QuizScore_5))
            {
                profileData.QuizScore_5 = quiz5Manager.score.ToString() + "/10";
                SetUserData(GetUserData); 
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
            if (string.IsNullOrEmpty(profileData.QuizScore_6))
            {
                profileData.QuizScore_6 = quiz6Manager.score.ToString() + "/10";
                SetUserData(GetUserData); 
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
            if (string.IsNullOrEmpty(profileData.QuizScore_7))
            {
                profileData.QuizScore_7 = quiz7Manager.score.ToString() + "/10";
                SetUserData(GetUserData); 
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
            if (string.IsNullOrEmpty(profileData.QuizScore_8))
            {
                profileData.QuizScore_8 = quiz8Manager.score.ToString() + "/10";
                SetUserData(GetUserData);
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
    public string Level_1_Timer;

    //Quiz Scores
    public string QuizScore_1;
    public string QuizScore_2;
    public string QuizScore_3;
    public string QuizScore_4;
    public string QuizScore_5;
    public string QuizScore_6;
    public string QuizScore_7;
    public string QuizScore_8;
}
