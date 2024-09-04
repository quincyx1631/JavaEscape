using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class QuizGenerator : MonoBehaviour
{
    public QuizSet[] quizSets; // Array of all quiz sets
    public CollectionUI collectionUI; // Reference to the CollectionUI script
    public Keypad keypad; // Reference to the Keypad script

    private QuizSet selectedSet;

    [System.Serializable]
    public class QuizSet
    {
        public GameObject cluePaper; // The clue paper for this set
        public GameObject[] quizPapers; // The quiz papers that match the clue
        public List<string> quizNames;
        public string password; // Password for this quiz set
    }

    private void Start()
    {
        // Perform quiz generation
        StartCoroutine(GenerateQuizzes());
    }

    private IEnumerator GenerateQuizzes()
    {
        Debug.Log($"QuizGenerator Start - Number of Quiz Sets: {quizSets.Length}");

        // Randomly select a quiz set
        int randomIndex = Random.Range(0, quizSets.Length);
        selectedSet = quizSets[randomIndex];
        Debug.Log($"Selected Clue: {selectedSet.cluePaper.name}");
        Debug.Log("Selected Quiz Papers: ");
        foreach (string quizName in selectedSet.quizNames)
        {
            Debug.Log(quizName);
        }

        // Check CollectionUI assignment
        if (collectionUI != null)
        {
            // Initialize collectionUI with the selected quiz names
            collectionUI.InitializeQuizzes(selectedSet.quizNames);
        }
        else
        {
            Debug.LogError("CollectionUI is not assigned in QuizGenerator.");
        }

        // Introduce a slight delay before disabling unnecessary clue and quiz papers
        yield return new WaitForSeconds(0.5f); // Half a second delay

        // Disable clue papers and quiz papers that are not part of the selected set
        foreach (var set in quizSets)
        {
            if (set != selectedSet)
            {
                // Disable clue papers that are not selected
                set.cluePaper.SetActive(false);
                Debug.Log($"Disabled Clue Paper: {set.cluePaper.name}");
            }

            // Disable quiz papers that do not match the selected quiz names
            foreach (var paper in set.quizPapers)
            {
                if (!selectedSet.quizNames.Contains(paper.name))
                {
                    paper.SetActive(false); // Disable the paper if it's not part of the selected quiz set
                    Debug.Log($"Disabled Quiz Paper: {paper.name}");
                }
            }
        }

        // Set the password for the keypad
        if (keypad != null)
        {
            keypad.SetPassword(selectedSet.password);
        }
        else
        {
            Debug.LogError("Keypad is not assigned in QuizGenerator.");
        }

        // Disable the entire GameObject after generating quizzes
        gameObject.SetActive(false);
    }
}
