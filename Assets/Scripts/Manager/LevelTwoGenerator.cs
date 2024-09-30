using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTwoGenerator : MonoBehaviour
{
    [Header("Quiz and Puzzle Setup")]
    public PuzzleSet[] puzzleSets; // Array to hold multiple sets of quiz, laptop, and keypad data
    private int currentSetIndex;

    [Header("UI Setup")]
    public TMP_Text[] questionTexts;
    public TMP_Text[] answerTexts;
    public TMP_Text clueText; // Text component for displaying the clue (laptop passphrase)

    [Header("References")]
    public KeypadNumbers keypadNumbers; // Reference to the KeypadNumbers script

    private void Start()
    {
        GeneratePuzzleSet();
    }

    [System.Serializable]
    public class PuzzleSet
    {
        public QuizSet quiz;                 // The quiz details
        public GameObject tarpulin;          // The tarpulin associated with this puzzle set
        public string laptopPassphrase;      // Clue for the laptop
        public string keypadPassword;        // Password for the keypad
    }

    [System.Serializable]
    public class QuizSet
    {
        [TextArea] public string[] questions;            // Questions for the quiz
        [TextArea] public string[] answers;              // Possible answers
        public int[] correctAnswers;          // Indexes of correct answers
    }

    // Generate the quiz set for the whiteboard and set corresponding passphrase for the laptop
    public void GeneratePuzzleSet()
    {
        currentSetIndex = Random.Range(0, puzzleSets.Length);
        PuzzleSet currentPuzzle = puzzleSets[currentSetIndex];

        // Display the questions and answers on the whiteboard
        for (int i = 0; i < questionTexts.Length; i++)
        {
            questionTexts[i].text = currentPuzzle.quiz.questions[i];
        }

        for (int i = 0; i < answerTexts.Length; i++)
        {
            answerTexts[i].text = currentPuzzle.quiz.answers[i];
        }

        // Display the clue for the laptop on the whiteboard
        clueText.text = currentPuzzle.laptopPassphrase;

        // Access the correct answers and pass them to the LineDrawer
        LineDrawer lineDrawer = FindObjectOfType<LineDrawer>();
        if (lineDrawer != null)
        {
            Debug.Log("Setting correct matches...");
            lineDrawer.UpdateCorrectAnswers(currentPuzzle.quiz.correctAnswers);
            Debug.Log("Correct matches set: " + string.Join(",", currentPuzzle.quiz.correctAnswers));
        }

        // Activate the associated tarpulin for the current puzzle set
        ActivateTarpulin(currentPuzzle.tarpulin);

        // Set the keypad password in the KeypadNumbers script
        if (keypadNumbers != null)
        {
            keypadNumbers.SetKeypadPassword(currentPuzzle.keypadPassword);
            Debug.Log("Keypad password updated in KeypadNumbers: " + currentPuzzle.keypadPassword);
        }
        else
        {
            Debug.LogError("KeypadNumbers reference is missing.");
        }

        // Disable the LevelTwoGenerator GameObject after generating the level
        gameObject.SetActive(false);
    }

    // Activate the specified tarpulin
    private void ActivateTarpulin(GameObject tarpulin)
    {
        // Deactivate all tarpulin sets
        foreach (var set in puzzleSets)
        {
            set.tarpulin.SetActive(false);
        }

        // Activate the current tarpulin
        tarpulin.SetActive(true);

        // Log the keypad password set for the current puzzle
        string keypadPassword = puzzleSets[currentSetIndex].keypadPassword;
        Debug.Log("Keypad password set for tarpulin: " + keypadPassword);
    }

    // Method to return the laptop password for the current puzzle set
    public string GetLaptopPassword()
    {
        return puzzleSets[currentSetIndex].laptopPassphrase;
    }
}
