using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class QuizManager6 : MonoBehaviour
{
    public List<QuestionAndAnswer6> QnA6;
    private List<QuestionAndAnswer6> originalQnA6; // Original list to reset questions
    public GameObject[] options6;
    public int currentQuestion;

    public Text QuestionTxt;

    public GameObject QuizPanel;
    public GameObject ScorePanel;

    public Text scoreTxt;
    int totalQuestions = 0;
    public int score = 0;

    private void Start()
    {
        totalQuestions = QnA6.Count;
        PlayerPrefs.DeleteKey("QuizRetry6");
        PlayerPrefs.DeleteKey("QuizScore6");
        // Create a copy of the original QnA6 list
        originalQnA6 = new List<QuestionAndAnswer6>(QnA6);

        // Check if there is a retry flag
        if (PlayerPrefs.HasKey("QuizRetry6"))
        {
            PlayerPrefs.DeleteKey("QuizRetry6"); // Clear the retry flag
            ScorePanel.SetActive(false);
            generateQuestion();
        }
        else if (PlayerPrefs.HasKey("QuizScore6"))
        {
            // If there is a saved score, show the ScorePanel
            score = PlayerPrefs.GetInt("QuizScore6");
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
        PlayerPrefs.SetInt("QuizRetry6", 1);
        PlayerPrefs.Save();

        // Reset the score and the question list
        score = 0;
        QnA6 = new List<QuestionAndAnswer6>(originalQnA6); // Reset QnA6 to the original list
        totalQuestions = QnA6.Count;

        // Reset the UI elements
        ScorePanel.SetActive(false);
        QuizPanel.SetActive(true);

        // Regenerate the first question
        generateQuestion();
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("QuizScore6", score);
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
            uiProfile.SetQuiz6Score();
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
        QnA6.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA6.RemoveAt(currentQuestion);
        generateQuestion();
    }

    private void setAnswers()
    {
        for (int i = 0; i < options6.Length; i++)
        {
            options6[i].GetComponent<AnswerScript6>().isCorrect = false;
            options6[i].transform.GetChild(0).GetComponent<Text>().text = QnA6[currentQuestion].Answers[i];

            if (QnA6[currentQuestion].CorrectAnswer == i + 1)
            {
                options6[i].GetComponent<AnswerScript6>().isCorrect = true;
            }
        }
    }


    void generateQuestion()
    {
        if (QnA6.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA6.Count);

            QuestionTxt.text = QnA6[currentQuestion].Question;
            setAnswers();
        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }
    }
}
