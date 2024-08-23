using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            profileData.level = 1; // Default level value
            profileData.playerName = "New Player"; // Default name
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
        SetUserData();
        GetUserData();
    }

    public void SetQuizScore()
    {
        QuizManager quizManager = FindObjectOfType<QuizManager>();
        if (quizManager != null)
        {
            if(profileData.QuizScore_1 == 0)
            {
                profileData.QuizScore_1 += quizManager.score;

                SetUserData();
            }
            else
            {
                Debug.Log("You already Completed the Quiz");
            }
        }

    }
}

[System.Serializable]
public class ProfileData
{
    public string playerName;
    public int level;
    public int score;
    public int QuizScore_1;
}

/*   //SCORING NG QUIZ AAYUSIN PA
     public int QuizScore_2;
     public int QuizScore_3;
     public int QuizScore_4;
     public int QuizScore_5;
     public int QuizScore_6;
     public int QuizScore_7;
     public int QuizScore_8;*/