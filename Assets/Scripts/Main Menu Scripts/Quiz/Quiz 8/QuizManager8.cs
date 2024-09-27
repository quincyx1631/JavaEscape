using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager8 : MonoBehaviour
{
    public List<QuestionAndAnswer8> QnA8;
    private List<QuestionAndAnswer8> originalQnA8; // Original list to reset questions
    public GameObject[] options8;
    public int currentQuestion;

    public Text QuestionTxt;

    public GameObject QuizPanel;
    public GameObject ScorePanel;

    public Text scoreTxt;
    int totalQuestions = 0;
    public int score = 0;

    private void Start()
    {
        totalQuestions = QnA8.Count;
        PlayerPrefs.DeleteKey("QuizRetry8");
        PlayerPrefs.DeleteKey("QuizScore8");
        // Create a copy of the original QnA8 list
        originalQnA8 = new List<QuestionAndAnswer8>(QnA8);

        // Check if there is a retry flag
        if (PlayerPrefs.HasKey("QuizRetry8"))
        {
            PlayerPrefs.DeleteKey("QuizRetry8"); // Clear the retry flag
            ScorePanel.SetActive(false);
            generateQuestion();
        }
        else if (PlayerPrefs.HasKey("QuizScore8"))
        {
            // If there is a saved score, show the ScorePanel
            score = PlayerPrefs.GetInt("QuizScore8");
            ShowScorePanel();
        }
        else
        {
            // Start quiz normally
            ScorePanel.SetActive(false);
            generateQuestion();
        }
    }
    public void retry()
    {
        // Set the retry flag and save it
        PlayerPrefs.SetInt("QuizRetry8", 1);
        PlayerPrefs.Save();

        // Reset the score and the question list
        score = 0;
        QnA8 = new List<QuestionAndAnswer8>(originalQnA8); // Reset QnA8 to the original list
        totalQuestions = QnA8.Count;

        // Reset the UI elements
        ScorePanel.SetActive(false);
        QuizPanel.SetActive(true);

        // Regenerate the first question
        generateQuestion();
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("QuizScore8", score);
        PlayerPrefs.Save();
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        // Show the score panel
        ShowScorePanel();

        // Wait for 1 second
        yield return new WaitForSecondsRealtime(1f);

        UserProfile uiProfile = FindAnyObjectByType<UserProfile>();

        if (uiProfile != null)
        {
            Debug.Log("UserProfile found. Setting quiz score...");
            uiProfile.SetQuiz8Score();
            Debug.Log("Quiz score set successfully.");
        }
        else
        {
            Debug.LogError("UserProfile not found in the scene.");
        }
    }

    void ShowScorePanel()
    {
        QuizPanel.SetActive(false);
        ScorePanel.SetActive(true);
        scoreTxt.text = "Score: " + score + "/" + totalQuestions;
    }

    public void correct()
    {
        score += 1;
        QnA8.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA8.RemoveAt(currentQuestion);
        generateQuestion();
    }

    private void setAnswers()
    {
        for (int i = 0; i < options8.Length; i++)
        {
            options8[i].GetComponent<AnswerScript8>().isCorrect = false;
            options8[i].transform.GetChild(0).GetComponent<Text>().text = QnA8[currentQuestion].Answers[i];

            if (QnA8[currentQuestion].CorrectAnswer == i + 1)
            {
                options8[i].GetComponent<AnswerScript8>().isCorrect = true;
            }
        }
    }


    void generateQuestion()
    {
        if (QnA8.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA8.Count);

            QuestionTxt.text = QnA8[currentQuestion].Question;
            setAnswers();
        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }
    }
}