using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizLevelSelector : MonoBehaviour
{
    public Button[] Quizbuttons;
    public GameObject QuizWeekSelection;
    public GameObject[] quizLevels;

    private void Awake()
    {
        int ReferenceLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < Quizbuttons.Length; i++)
        {
            Quizbuttons[i].interactable = false;
        }
        for (int i = 0; i < ReferenceLevel; i++)
        {
            Quizbuttons[i].interactable = true;
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
