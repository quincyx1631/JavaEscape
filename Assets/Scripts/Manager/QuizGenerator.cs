// QuizGenerator.cs
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class QuizGenerator : MonoBehaviour
{
    public QuizSet[] quizSets;
    public CollectionUI collectionUI;
    public Keypad keypad;
    private QuizSet selectedSet;

    [System.Serializable]
    public class QuizSet
    {
        public GameObject cluePaper;
        public GameObject[] quizPapers;
        public List<string> quizNames = new List<string>(); // Initialize the list
        public string password;

        // Validate the quiz set
        public bool IsValid()
        {
            if (cluePaper == null)
            {
                Debug.LogError("Clue paper is not assigned!");
                return false;
            }
            if (quizPapers == null || quizPapers.Length == 0)
            {
                Debug.LogError($"Quiz papers array is empty for clue {cluePaper.name}!");
                return false;
            }
            if (quizNames == null || quizNames.Count == 0)
            {
                Debug.LogError($"Quiz names list is empty for clue {cluePaper.name}!");
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                Debug.LogError($"Password is empty for clue {cluePaper.name}!");
                return false;
            }
            return true;
        }
    }

    private void Awake()
    {
        // Validate components
        if (collectionUI == null)
        {
            Debug.LogError("CollectionUI is not assigned in QuizGenerator!");
            enabled = false;
            return;
        }

        if (keypad == null)
        {
            Debug.LogError("Keypad is not assigned in QuizGenerator!");
            enabled = false;
            return;
        }

        if (quizSets == null || quizSets.Length == 0)
        {
            Debug.LogError("No quiz sets defined in QuizGenerator!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        // Ensure everything is initialized before starting
        if (enabled)
        {
            StartCoroutine(GenerateQuizzes());
        }
    }

    private IEnumerator GenerateQuizzes()
    {
        Debug.Log($"QuizGenerator Start - Number of Quiz Sets: {quizSets.Length}");

        // Create a list of valid quiz sets
        List<QuizSet> validSets = new List<QuizSet>();
        foreach (var set in quizSets)
        {
            if (set.IsValid())
            {
                validSets.Add(set);
            }
        }

        if (validSets.Count == 0)
        {
            Debug.LogError("No valid quiz sets found!");
            yield break;
        }

        // Randomly select a quiz set from valid sets
        int randomIndex = Random.Range(0, validSets.Count);
        selectedSet = validSets[randomIndex];

        Debug.Log($"Selected Clue: {selectedSet.cluePaper.name}");
        Debug.Log("Selected Quiz Names:");
        foreach (string quizName in selectedSet.quizNames)
        {
            Debug.Log($"- {quizName}");
        }

        // Create a new list with the quiz names to ensure it's not null
        List<string> quizNamesToInitialize = new List<string>(selectedSet.quizNames);

        try
        {
            // Initialize collectionUI with the selected quiz names
            collectionUI.InitializeQuizzes(quizNamesToInitialize);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error initializing quizzes: {e.Message}\n{e.StackTrace}");
            yield break;
        }

        // Add a small delay to ensure UI updates properly
        yield return new WaitForSeconds(0.5f);

        // Disable unused quiz sets
        foreach (var set in quizSets)
        {
            if (set != selectedSet)
            {
                if (set.cluePaper != null)
                {
                    set.cluePaper.SetActive(false);
                    Debug.Log($"Disabled Clue Paper: {set.cluePaper.name}");
                }

                if (set.quizPapers != null)
                {
                    foreach (var paper in set.quizPapers)
                    {
                        if (paper != null && !selectedSet.quizNames.Contains(paper.name))
                        {
                            paper.SetActive(false);
                            Debug.Log($"Disabled Quiz Paper: {paper.name}");
                        }
                    }
                }
            }
        }

        // Set the password for the keypad
        keypad.SetPassword(selectedSet.password);

        // Disable this generator after setup is complete
        gameObject.SetActive(false);
    }
}