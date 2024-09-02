using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript5 : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager5 quizManager5;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer at time: " + Time.time);
            quizManager5.correct();
        }
        else
        {
            Debug.Log("Wrong Answer at time: " + Time.time);
            quizManager5.wrong(); // Call a separate method for wrong answers
        }
    }
}