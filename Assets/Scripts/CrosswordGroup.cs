using System.Collections.Generic;
using UnityEngine;

public class CrosswordGroup : MonoBehaviour
{
    public string targetWord;
    public List<CrosswordButton> buttons;

    private void Update()
    {
        // If all letters in the group are correct, lock the group
        if (IsWordCorrect())
        {
            LockGroup(); // Lock the group if the word is complete and correct
        }
    }

    // Check if the word is complete and correct
    public bool IsWordCorrect()
    {
        foreach (var button in buttons)
        {
            if (!button.IsCorrect())
            {
                return false; // Return false if any letter is incorrect
            }
        }
        return true; // Return true when all letters are correct
    }

    // Lock all buttons in the group when the word is complete and correct
    public void LockGroup()
    {
        foreach (var button in buttons)
        {
            button.LockButton(); // Lock each button in the group
        }
    }
}
