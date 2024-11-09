using System.Collections.Generic;
using UnityEngine;

public class CrosswordGroup : MonoBehaviour
{
    public List<CrosswordInputField> inputFields; // List of input fields

    // Check if the group is complete
    public bool IsGroupComplete()
    {
        // Make sure inputFields is not null and contains elements
        if (inputFields == null || inputFields.Count == 0)
        {
            Debug.LogWarning("No input fields found in the group.");
            return false;
        }

        foreach (var inputField in inputFields)
        {
            // Check if the inputField is not null and has a text assigned
            if (inputField == null || inputField.inputField == null)
            {
                Debug.LogWarning("Missing input field or TMP_InputField reference.");
                return false; // Return false if any input field is missing or not assigned
            }
            if (string.IsNullOrEmpty(inputField.inputField.text))
            {
                return false; // Return false if any letter is missing
            }
            if (!inputField.IsCorrect())
            {
                return false; // Return false if any letter is incorrect
            }
        }

        return true; // Return true only if the word is correct and complete
    }

    // Lock the entire group once the word is correct
    public void LockGroup()
    {
        foreach (var inputField in inputFields)
        {
            if (inputField != null)
            {
                inputField.LockInput(); // Lock each input field in the group
            }
        }
    }
}
