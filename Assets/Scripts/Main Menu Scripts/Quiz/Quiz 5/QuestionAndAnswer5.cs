using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuestionAndAnswer5
{
    [TextAreaAttribute]
    public string Question;
    public string[] Answers;
    public int CorrectAnswer;
}
