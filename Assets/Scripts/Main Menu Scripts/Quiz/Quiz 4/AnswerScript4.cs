using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript4 : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager4 quizManager4;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer at time: " + Time.time);
            quizManager4.correct();
        }
        else
        {
            Debug.Log("Wrong Answer at time: " + Time.time);
            quizManager4.wrong(); // Call a separate method for wrong answers
        }
    }
}