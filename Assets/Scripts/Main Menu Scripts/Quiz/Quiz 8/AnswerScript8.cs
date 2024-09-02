using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript8 : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager8 quizManager8;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer at time: " + Time.time);
            quizManager8.correct();
        }
        else
        {
            Debug.Log("Wrong Answer at time: " + Time.time);
            quizManager8.wrong(); // Call a separate method for wrong answers
        }
    }
}