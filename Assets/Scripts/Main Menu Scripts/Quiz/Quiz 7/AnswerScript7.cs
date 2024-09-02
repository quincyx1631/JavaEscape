using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript7 : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager7 quizManager7;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer at time: " + Time.time);
            quizManager7.correct();
        }
        else
        {
            Debug.Log("Wrong Answer at time: " + Time.time);
            quizManager7.wrong(); // Call a separate method for wrong answers
        }
    }
}