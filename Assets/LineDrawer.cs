using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

    public int[] correctMatches; // To define the correct matching pairs of questions and answers

    void Start()
    {
        answerAssigned = new Button[answerButtons.Length];

        for (int i = 0; i < questionButtons.Length; i++)
        {
            int index = i;
            questionButtons[i].onClick.AddListener(() => SelectQuestion(index));
        }

        foreach (var button in answerButtons)
        {
            button.onClick.AddListener(() => SelectAnswer(button));
        }

        clearButton.onClick.AddListener(ClearLastLine);
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
            GameObject lineObject = Instantiate(linePrefab, canvas.transform);
            RectTransform lineRect = lineObject.GetComponent<RectTransform>();

            Vector3 direction = endPos - startPos;
            float distance = direction.magnitude;

            // Set the position and rotation of the line
            lineRect.position = startPos;
            lineRect.sizeDelta = new Vector2(distance, 5f); // Line width (you can adjust)
            lineRect.pivot = new Vector2(0, 0.5f); // Set the pivot at the start
            lineRect.rotation = Quaternion.FromToRotation(Vector3.right, direction);

            return lineObject;
        }
        return null;
    }

    Vector3 GetButtonPosition(Button button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rectTransform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out var localPoint);
        return canvas.transform.TransformPoint(localPoint);
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

    void ClearLastLine()
    {
        if (lineStack.Count > 0)
        {
            GameObject lastLine = lineStack.Pop();
            if (lastLine != null)
            {
                Destroy(lastLine);
            }

            UndoAssignment();
        }
    }

    void UndoAssignment()
    {
        if (lineStack.Count < answerAssigned.Length)
        {
            for (int i = 0; i < answerAssigned.Length; i++)
            {
                if (answerAssigned[i] != null)
                {
                    answerAssigned[i] = null;
                    Debug.Log($"Cleared assignment for Question {i + 1}");
                    break;
                }
            }
        }

        hasStartPosition = false;
        selectedQuestionIndex = -1;
    }

    public void CheckMatches()
    {
        bool allCorrect = true;

        for (int i = 0; i < questionButtons.Length; i++)
        {
            int assignedAnswerIndex = System.Array.IndexOf(answerButtons, answerAssigned[i]);

            if (assignedAnswerIndex != correctMatches[i])
            {
                allCorrect = false;
                Debug.Log($"Question {i + 1} is incorrect.");
            }
            else
            {
                Debug.Log($"Question {i + 1} is correct.");
            }
        }

        if (allCorrect)
        {
            Debug.Log("All matches are correct!");
        }
        else
        {
            Debug.Log("Some matches are incorrect. Try again!");
        }
    }

    public void ClearAllLines()
    {
        while (lineStack.Count > 0)
        {
            GameObject line = lineStack.Pop();
            if (line != null)
            {
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
}
