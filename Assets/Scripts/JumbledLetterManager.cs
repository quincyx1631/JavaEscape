using UnityEngine;

[System.Serializable]
public class LetterSet
{
    public LetterPlaceholder[] letterPlaceholders; // Array of letter placeholders for this set
    public string targetWord; // The target word to match (e.g., "DOG")
}

public class JumbledLetterManager : MonoBehaviour
{
    public LetterSet[] letterSets; // Array of letter sets

    private int currentSetIndex = 0; // Current letter set to check

    private void Start()
    {
        // Initialize or set up the first letter set if needed
        CheckLetters(); // Optionally check the letters when the game starts
    }

    public void CheckLetters()
    {
        if (currentSetIndex >= letterSets.Length)
        {
            Debug.LogError("Current set index is out of range.");
            return;
        }

        LetterSet currentSet = letterSets[currentSetIndex];
        bool allCorrect = true;

        for (int i = 0; i < currentSet.letterPlaceholders.Length; i++)
        {
            // Get the current LetterBox in the placeholder
            LetterBox currentLetterBox = currentSet.letterPlaceholders[i].GetPlacedItem()?.GetComponent<LetterBox>();

            // Check if the LetterBox exists and if the letter matches the target word
            if (currentLetterBox != null)
            {
                char placedLetter = currentLetterBox.gameObject.name[0]; // Assuming the name of the LetterBox is the letter itself
                if (i < currentSet.targetWord.Length && placedLetter == currentSet.targetWord[i])
                {
                    // Correct letter
                    currentSet.letterPlaceholders[i].GetComponent<Renderer>().material.color = Color.green; // Change color to green
                }
                else
                {
                    // Incorrect letter
                    currentSet.letterPlaceholders[i].GetComponent<Renderer>().material.color = Color.red; // Change color to red
                    allCorrect = false; // Not all letters are correct
                }
            }
            else
            {
                // No letter placed in this placeholder
                allCorrect = false; // Mark as not correct
                currentSet.letterPlaceholders[i].GetComponent<Renderer>().material.color = Color.white; // Reset color to white
            }
        }

        // If all letters are correct, untag all placeholders and LetterBoxes
        if (allCorrect)
        {
            Debug.Log("All letters are correct!");
            foreach (var placeholder in currentSet.letterPlaceholders)
            {
                placeholder.tag = "Untagged"; // Set placeholder tag to Untagged
                if (placeholder.GetPlacedItem() != null)
                {
                    placeholder.GetPlacedItem().tag = "Untagged"; // Set LetterBox tag to Untagged
                }
            }

            // Optionally, move to the next set or reset the current set
            currentSetIndex++; // Increment to the next set
            if (currentSetIndex < letterSets.Length)
            {
                Debug.Log("Moving to the next letter set.");
            }
            else
            {
                Debug.Log("All letter sets completed.");
                // Implement any completion logic here
            }
        }
    }
}
