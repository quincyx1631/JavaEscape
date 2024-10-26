using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelThreeGenerator : MonoBehaviour
{
    public LevelSet[] levelSets; // Array of all level sets
    public LaptopTwoUI laptopTwoUI; // Reference to the LaptopTwoUI script
    public RiddleUI riddleUI; // Reference to the RiddleUI script
    public KeypadNumbers keypad; // Reference to the KeypadNumbers script
    public TextMeshProUGUI riddleTextMesh; // Reference to the TextMeshPro component for the riddle

    private LevelSet selectedSet;

    [System.Serializable]
    public class LevelSet
    {
        public GameObject[] quizPapers; // The quiz papers that match the set
        public List<string> correctAnswers; // List of correct answers for the LaptopTwoUI
        public string riddleAnswer; // Correct answer for the riddle
        public string nextTaskPassword; // Password for the next task
        public string keypadPassword; // Password for the keypad
        [TextArea] public string riddleText; // Text of the riddle to be displayed
    }

    private void Start()
    {
        // Start level generation
        StartCoroutine(GenerateLevel());
    }

    private IEnumerator GenerateLevel()
    {
        Debug.Log($"LevelThreeGenerator Start - Number of Level Sets: {levelSets.Length}");

        // Randomly select a level set
        int randomIndex = Random.Range(0, levelSets.Length);
        selectedSet = levelSets[randomIndex];
        Debug.Log($"Selected Level Set: {randomIndex}");

        // Initialize LaptopTwoUI with correct answers
        if (laptopTwoUI != null)
        {
            laptopTwoUI.correctAnswers = selectedSet.correctAnswers;
            Debug.Log("Correct Answers for LaptopTwoUI Set");
        }
        else
        {
            Debug.LogError("LaptopTwoUI is not assigned in LevelThreeGenerator.");
        }

        // Initialize RiddleUI with correct answer and next task password
        if (riddleUI != null)
        {
            riddleUI.correctAnswer = selectedSet.riddleAnswer;
            riddleUI.nextTaskPassword = selectedSet.nextTaskPassword;
            Debug.Log("Correct Answer for RiddleUI Set");
        }
        else
        {
            Debug.LogError("RiddleUI is not assigned in LevelThreeGenerator.");
        }

        // Set the riddle text for the TextMeshPro component
        if (riddleTextMesh != null)
        {
            riddleTextMesh.text = selectedSet.riddleText;
            Debug.Log("Riddle Text Set");
        }
        else
        {
            Debug.LogError("Riddle TextMeshPro component is not assigned.");
        }

        // Set the password for the Keypad
        if (keypad != null)
        {
            keypad.SetKeypadPassword(selectedSet.keypadPassword);
            Debug.Log("Keypad Password Set");
        }
        else
        {
            Debug.LogError("Keypad is not assigned in LevelThreeGenerator.");
        }

        // Introduce a slight delay before disabling unnecessary quiz papers
        yield return new WaitForSeconds(0.5f);

        // Disable all quiz papers that are not part of the selected set
        foreach (var set in levelSets)
        {
            if (set != selectedSet)
            {
                foreach (var paper in set.quizPapers)
                {
                    paper.SetActive(false); // Disable all papers that do not match the selected quiz set
                    Debug.Log($"Disabled Quiz Paper: {paper.name}");
                }
            }
        }

        // Disable the entire GameObject after generating the level
        gameObject.SetActive(false);
    }
}
