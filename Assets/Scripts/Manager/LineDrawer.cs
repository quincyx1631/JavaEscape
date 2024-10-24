using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab; // Prefab for the line (Image object)
    public Canvas canvas; // Reference to the canvas

    public Button[] questionButtons; // Array for the question buttons
    public Button[] answerButtons;   // Array for the answer buttons
    public Button clearButton; // Reference to the clear button

    private Vector3 startPos, endPos; // To hold the start (question) and end (answer) positions
    private bool hasStartPosition = false; // To track if the start position is set

    private Stack<GameObject> lineStack = new Stack<GameObject>(); // Stack to keep track of drawn lines
    private Button[] answerAssigned; // Array to track if an answer has been assigned a line
    private int selectedQuestionIndex = -1; // Index to track the selected question

    private int[] correctMatches; // To define the correct matching pairs of questions and answers
    public AlertUI alertUI; // Reference to the AlertUI component
    public GameObject blackboard; // Reference to the blackboard GameObject

    public TextMeshProUGUI[] blackboardTexts; // Array for multiple TextMeshPro components
    public TextMeshProUGUI clue; // Reference to the clue TextMeshPro component
    public GameObject blackboardUI;

    private Outline blackboardOutline; // Reference to the blackboard's Outline component

    public string drawSoundName;
    public string eraseSoundName;

    [Header("Line Settings")]
    public float lineWidth = 5f; // Adjustable line width
    public float lineLengthMultiplier = 1.0f;

    void Start()
    {
        answerAssigned = new Button[answerButtons.Length];

        // Initialize the blackboardOutline reference
        blackboardOutline = blackboard.GetComponent<Outline>();

        // Ensure that the blackboard outline is disabled at the start
        if (blackboardOutline != null)
        {
            blackboardOutline.enabled = false;
        }

        for (int i = 0; i < questionButtons.Length; i++)
        {
            int index = i;
            questionButtons[i].onClick.AddListener(() => SelectQuestion(index));
        }

        foreach (var button in answerButtons)
        {
            button.onClick.AddListener(() => SelectAnswer(button));
        }

        clearButton.onClick.AddListener(ClearAllLines); // Clear all lines when clearButton is clicked
    }

    void Update()
    {
        // Check if Escape is pressed and ensure that the blackboard outline remains disabled
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (blackboardOutline != null)
            {
                blackboardOutline.enabled = false;
            }
        }
    }

    void SelectQuestion(int questionIndex)
    {
        startPos = GetButtonPosition(questionButtons[questionIndex]);
        hasStartPosition = true;
        selectedQuestionIndex = questionIndex;
    }

    void SelectAnswer(Button selectedAnswerButton)
    {
        if (hasStartPosition && selectedQuestionIndex != -1)
        {
            if (IsAnswerAlreadyAssigned(selectedAnswerButton))
            {
                Debug.Log("This answer already has a line.");
                return;
            }

            endPos = GetButtonPosition(selectedAnswerButton);
            GameObject newLine = CreateLine();
            if (newLine != null)
            {
                lineStack.Push(newLine);
            }

            answerAssigned[selectedQuestionIndex] = selectedAnswerButton;
            hasStartPosition = false;
            selectedQuestionIndex = -1;
        }
    }

    GameObject CreateLine()
    {
        if (linePrefab != null)
        {
            if (!string.IsNullOrEmpty(drawSoundName))
            {
                AudioManager.Instance.Play(drawSoundName);
            }
            GameObject lineObject = Instantiate(linePrefab, canvas.transform);
            RectTransform lineRect = lineObject.GetComponent<RectTransform>();

            Vector3 direction = endPos - startPos;
            float distance = direction.magnitude;

            // Set the length using the multiplier from the inspector
            lineRect.sizeDelta = new Vector2(distance * lineLengthMultiplier, lineWidth); // Length = distance * multiplier, Width = lineWidth

            lineRect.position = startPos;
            lineRect.pivot = new Vector2(0, 0.5f);
            lineRect.rotation = Quaternion.FromToRotation(Vector3.right, direction);

            return lineObject;
        }

        return null;
    }

    Vector3 GetButtonPosition(Button button)
    {
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        return buttonRect.position; // Get the world position of the button
    }

    bool IsAnswerAlreadyAssigned(Button selectedAnswerButton)
    {
        foreach (var assignedButton in answerAssigned)
        {
            if (assignedButton == selectedAnswerButton)
            {
                return true;
            }
        }
        return false;
    }

    // This method will clear all lines when the clear button is pressed
    public void ClearAllLines()
    {
        while (lineStack.Count > 0)
        {
            GameObject line = lineStack.Pop();
            if (line != null)
            {
                if (!string.IsNullOrEmpty(eraseSoundName))
                {
                    AudioManager.Instance.Play(eraseSoundName);
                }
                Destroy(line);
            }
        }

        ResetAnswerAssignments();
    }

    void ResetAnswerAssignments()
    {
        for (int i = 0; i < answerAssigned.Length; i++)
        {
            answerAssigned[i] = null;
        }

        hasStartPosition = false;
        selectedQuestionIndex = -1;
    }

    public void CheckMatches()
    {
        if (correctMatches == null || correctMatches.Length == 0)
        {
            Debug.LogError("Correct matches have not been set.");
            return;
        }

        bool allCorrect = true;

        // Now check against the correctMatches defined in this class
        for (int i = 0; i < questionButtons.Length; i++)
        {
            int assignedAnswerIndex = System.Array.IndexOf(answerButtons, answerAssigned[i]);

            // Check if assigned answer is correct
            if (assignedAnswerIndex != correctMatches[i])
            {
                allCorrect = false;
                alertUI.ShowAlert("Incorrect answer, Try Again!!"); // Show alert for incorrect answer
                return; // Exit the method if there's an incorrect answer
            }
        }

        // If all answers are correct, proceed to clear lines and handle UI
        if (allCorrect)
        {
            ClearAllLines(); // Clear all drawn lines
            CorrectUIController.Instance.ShowCorrectUI();

            // Disable the UI elements related to line drawing
            foreach (var button in questionButtons)
            {
                button.interactable = false;
            }

            foreach (var button in answerButtons)
            {
                button.interactable = false;
            }

            clearButton.interactable = false;

            // Optionally hide or deactivate the whole line-drawing UI if needed
            blackboardUI.SetActive(false);

            // Ensure the blackboard's Outline stays disabled after correct answer
            if (blackboardOutline != null)
            {
                blackboardOutline.enabled = false;
            }

            // Handle camera switch and clue logic...
            CameraSwitch cameraSwitch = FindObjectOfType<CameraSwitch>();
            if (cameraSwitch != null)
            {
                blackboard.tag = "Untagged"; // Change the tag of the blackboard

                // Hide blackboard texts
                foreach (var text in blackboardTexts)
                {
                    if (text != null)
                    {
                        text.gameObject.SetActive(false);
                    }
                }

                // Show the clue after all correct matches
                if (clue != null)
                {
                    clue.gameObject.SetActive(true);
                }

                cameraSwitch.SwitchToMainCamera();
            }
        }
    }

    // Public method to set correct answers from other scripts
    public void SetCorrectAnswers(int[] correctAnswers)
    {
        this.correctMatches = correctAnswers;
    }

    // Public method to set correct answers from LevelTwoGenerator
    public void UpdateCorrectAnswers(int[] correctAnswers)
    {
        this.correctMatches = correctAnswers;
        Debug.Log("Correct matches updated: " + string.Join(",", correctMatches));
    }
}
