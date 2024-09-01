using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript2 : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager2 quizManager2;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer at time: " + Time.time);
            quizManager2.correct();
        }
        else
        {
            Debug.Log("Wrong Answer at time: " + Time.time);
            quizManager2.wrong(); // Call a separate method for wrong answers
        }
    }
}