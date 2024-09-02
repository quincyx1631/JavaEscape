using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript6 : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager6 quizManager6;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer at time: " + Time.time);
            quizManager6.correct();
        }
        else
        {
            Debug.Log("Wrong Answer at time: " + Time.time);
            quizManager6.wrong(); // Call a separate method for wrong answers
        }
    }
}