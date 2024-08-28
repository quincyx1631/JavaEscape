using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizLevelSelector : MonoBehaviour
{
    public Button[] Quizbuttons;
    public GameObject QuizWeekSelection;
    public GameObject[] quizLevels;
    private int currentQuiz;

    void Start()
    {
        // Subscribe to the profile data updated event
        UserProfile.OnProfileDataUpdated.AddListener(UpdateQuizSelection);

        // Initialize the level buttons based on current profile data
        if (UserProfile.Instance != null)
        {
            UpdateQuizSelection(UserProfile.Instance.profileData);
        }
    }

    void UpdateQuizSelection(ProfileData profileData)
    {
        currentQuiz = profileData.level;

        // Ensure Level 1 is always open
        Quizbuttons[0].interactable = true;

        // Loop through the level buttons and set interactability
        for (int i = 1; i < Quizbuttons.Length; i++)
        {
            if (i < currentQuiz)
            {
                Quizbuttons[i].interactable = true;
            }
            else
            {
                Quizbuttons[i].interactable = false;
            }
        }
    }

    public void OpenQuizLevel(int QuizWeekNo)
    {
        string quizWeekLabel = "Level " + QuizWeekNo;

        // Disable all quiz level GameObjects
        foreach (GameObject quizLevel in quizLevels)
        {
            quizLevel.SetActive(false);
        }

        // Assuming the naming convention or some other mechanism maps the quizWeekLabel to a specific GameObject
        // Activate the selected quiz level GameObject
        quizLevels[QuizWeekNo - 1].SetActive(true); // Assuming quizWeekNo starts from 1

        // Close the level selection UI
        QuizWeekSelection.SetActive(false);
    }
}
