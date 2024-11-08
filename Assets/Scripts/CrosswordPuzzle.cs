using System.Collections.Generic;
using UnityEngine;

public class CrosswordPuzzle : MonoBehaviour
{
    public List<CrosswordGroup> crosswordGroups;

    private void Update()
    {
        foreach (var group in crosswordGroups)
        {
            if (group.IsWordCorrect())
            {
                group.LockGroup(); // Lock the group if the word is correct
            }
        }
    }
}
