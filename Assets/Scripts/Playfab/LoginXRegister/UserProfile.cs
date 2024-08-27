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

    //SCORE NEEDED FOR QUIZ I NEED TO FIX THIS!!
    public int score = 10;

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
            profileData.score = 0; // Default score
            profileData.QuizScore_1 = 0; // Default quiz score
            
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
        profileData.level += 1;

        Door timerLevel = FindAnyObjectByType<Door>();
        string timerLevelSave = timerLevel.finalElapsedTime.ToString();

        profileData.Level_1_Timer = timerLevelSave;
        SetUserData();
        GetUserData();
    }

    public void SetQuizScore()
    {
        QuizManager quizManager = FindObjectOfType<QuizManager>();
        if (quizManager != null)
        {
            if (profileData.QuizScore_1 == 0)
            {
                profileData.QuizScore_1 += quizManager.score;

                SetUserData(GetUserData);
            }
            else
            {
                Debug.Log("You already Completed the Quiz");
            }
        }
    }

    public void SetPlayerName(string displayName)
    {
        // Check if the display name is not null or empty
        if (!string.IsNullOrEmpty(displayName))
        {
            profileData.playerName = displayName;
            SetUserData(GetUserData);
            UserAccountManager.Instance.SetDisplayName(displayName);
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
    public string playerName;
    public string Student_Section;
    public int level;
    public int score;
    public int QuizScore_1;
    public string Level_1_Timer;
    
}

/*public class StudentData
{
    public string Student_Name;
    public string Student_Section;
}*/

/*   //SCORING NG QUIZ AAYUSIN PA
     public int QuizScore_2;
     public int QuizScore_3;
     public int QuizScore_4;
     public int QuizScore_5;
     public int QuizScore_6;
     public int QuizScore_7;
     public int QuizScore_8;*/
