using System.Collections.Generic;
using UnityEngine;

public class CrosswordGroup : MonoBehaviour
{
    public List<CrosswordInputField> inputFields; // List of input fields
 

    // Check if the group is complete and correct
    public bool IsGroupComplete()
    {
        if (inputFields == null || inputFields.Count == 0)
        {
            Debug.LogWarning("No input fields found in the group.");
            return false;
        }

        foreach (var inputField in inputFields)
        {
            if (inputField == null || inputField.inputField == null)
            {
                Debug.LogWarning("Missing input field or TMP_InputField reference.");
                return false;
            }
            if (string.IsNullOrEmpty(inputField.inputField.text))
            {
                return false;
            }
            if (!inputField.IsCorrect())
            {
                return false;
            }
        }
        return true;
    }

    // Lock the entire group once the word is correct
    public void LockGroup()
    {
        foreach (var inputField in inputFields)
        {
            if (inputField != null)
            {
                inputField.LockInput();
            }
        }

       
    }
}
