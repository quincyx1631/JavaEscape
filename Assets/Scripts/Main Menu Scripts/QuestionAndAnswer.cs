using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestionAndAnswer
{

    [TextAreaAttribute]
    public string Question;
    public string[] Answers;
    public int CorrectAnswer;
}