using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class QuizManager4 : MonoBehaviour
{
    public List<QuestionAndAnswer4> QnA4;
    private List<QuestionAndAnswer4> originalQnA4; // Original list to reset questions
    public GameObject[] options4;
    public int currentQuestion;

    public Text QuestionTxt;

    public GameObject QuizPanel;
    public GameObject ScorePanel;

    public Text scoreTxt;
    int totalQuestions = 0;
    public int score = 0;

    private void Start()
    {
        totalQuestions = QnA4.Count;
        PlayerPrefs.DeleteKey("QuizRetry4");
        PlayerPrefs.DeleteKey("QuizScore4");
        // Create a copy of the original QnA4 list
        originalQnA4 = new List<QuestionAndAnswer4>(QnA4);

        // Check if there is a retry flag
        if (PlayerPrefs.HasKey("QuizRetry4"))
        {
            PlayerPrefs.DeleteKey("QuizRetry4"); // Clear the retry flag
            ScorePanel.SetActive(false);
            generateQuestion();
        }
        else if (PlayerPrefs.HasKey("QuizScore4"))
        {
            // If there is a saved score, show the ScorePanel
            score = PlayerPrefs.GetInt("QuizScore4");
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
        PlayerPrefs.SetInt("QuizRetry4", 1);
        PlayerPrefs.Save();

        // Reset the score and the question list
        score = 0;
        QnA4 = new List<QuestionAndAnswer4>(originalQnA4); // Reset QnA4 to the original list
        totalQuestions = QnA4.Count;

        // Reset the UI elements
        ScorePanel.SetActive(false);
        QuizPanel.SetActive(true);

        // Regenerate the first question
        generateQuestion();
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("QuizScore4", score);
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
            uiProfile.SetQuiz4Score();
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
        QnA4.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA4.RemoveAt(currentQuestion);
        generateQuestion();
    }

    private void setAnswers()
    {
        for (int i = 0; i < options4.Length; i++)
        {
            options4[i].GetComponent<AnswerScript4>().isCorrect = false;
            options4[i].transform.GetChild(0).GetComponent<Text>().text = QnA4[currentQuestion].Answers[i];

            if (QnA4[currentQuestion].CorrectAnswer == i + 1)
            {
                options4[i].GetComponent<AnswerScript4>().isCorrect = true;
            }
        }
    }


    void generateQuestion()
    {
        if (QnA4.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA4.Count);

            QuestionTxt.text = QnA4[currentQuestion].Question;
            setAnswers();
        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }
    }
}
