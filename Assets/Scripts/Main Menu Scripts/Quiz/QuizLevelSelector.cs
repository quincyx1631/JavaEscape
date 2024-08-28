using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizLevelSelector : MonoBehaviour
{
    public Button[] Quizbuttons;
    private int currentQuiz;

    private void Start()
    {
        // Subscribe to the profile data updated event
        UserProfile.OnProfileDataUpdated.AddListener(UpdateQuizSelection);

        // Initialize the level buttons based on current profile data
        if (UserProfile.Instance != null)
        {
            UpdateQuizSelection(UserProfile.Instance.profileData);
        }
    }

    public void UpdateQuizSelection(ProfileData profileData)
    {
        currentQuiz = profileData.level;

        // Ensure Level 1 is always open
        Quizbuttons[0].interactable = true;

        // Loop through the level buttons and set interactability
        for (int i = 1; i < Quizbuttons.Length; i++)
        {
            if (i <= currentQuiz)
            {
                Quizbuttons[i].interactable = true;
            }
            else
            {
                Quizbuttons[i].interactable = false;
            }
        }
    }
}
