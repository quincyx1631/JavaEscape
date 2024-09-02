using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuestionAndAnswer4
{
    [TextAreaAttribute]
    public string Question;
    public string[] Answers;
    public int CorrectAnswer;
}
