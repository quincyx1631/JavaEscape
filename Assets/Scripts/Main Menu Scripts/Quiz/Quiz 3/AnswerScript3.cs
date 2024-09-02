using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript3 : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager3 quizManager3;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer at time: " + Time.time);
            quizManager3.correct();
        }
        else
        {
            Debug.Log("Wrong Answer at time: " + Time.time);
            quizManager3.wrong(); // Call a separate method for wrong answers
        }
    }
}