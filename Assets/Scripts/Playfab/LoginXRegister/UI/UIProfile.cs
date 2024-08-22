using System;
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
/*      UserAccountManager.OnSignInSuccess.AddListener(SignIn);*/ //TUTORIAL TO GALING

        UserProfile.OnProfileDataUpdated.AddListener(ProfileDataUpdated);
    }

    void OnDisable()
    {
        UserProfile.OnProfileDataUpdated.RemoveListener(ProfileDataUpdated);
    }

/*    void SignIn()
    {
        
    }*/

    void ProfileDataUpdated(ProfileData profileData)
    {
        int level = profileData.level;

        playerNameText.text = profileData.playerName;
        playerLevelText.text = level.ToString();
        playerScoreText.text = UserProfile.Instance.score.ToString();
        playerQuiz1Text.text = "Quiz 1: " + profileData.QuizScore_1.ToString();
    }
}
