using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager5 : MonoBehaviour
{
    public List<QuestionAndAnswer5> QnA5;
    private List<QuestionAndAnswer5> originalQnA5; // Original list to reset questions
    public GameObject[] options5;
    public int currentQuestion;

    public Text QuestionTxt;

    public GameObject QuizPanel;
    public GameObject ScorePanel;

    public Text scoreTxt;
    int totalQuestions = 0;
    public int score = 0;

    private void Start()
    {
        totalQuestions = QnA5.Count;
        PlayerPrefs.DeleteKey("QuizRetry5");
        PlayerPrefs.DeleteKey("QuizScore5");
        // Create a copy of the original QnA5 list
        originalQnA5 = new List<QuestionAndAnswer5>(QnA5);

        // Check if there is a retry flag
        if (PlayerPrefs.HasKey("QuizRetry5"))
        {
            PlayerPrefs.DeleteKey("QuizRetry5"); // Clear the retry flag
            ScorePanel.SetActive(false);
            generateQuestion();
        }
        else if (PlayerPrefs.HasKey("QuizScore5"))
        {
            // If there is a saved score, show the ScorePanel
            score = PlayerPrefs.GetInt("QuizScore5");
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
        PlayerPrefs.SetInt("QuizRetry5", 1);
        PlayerPrefs.Save();

        // Reset the score and the question list
        score = 0;
        QnA5 = new List<QuestionAndAnswer5>(originalQnA5); // Reset QnA5 to the original list
        totalQuestions = QnA5.Count;

        // Reset the UI elements
        ScorePanel.SetActive(false);
        QuizPanel.SetActive(true);

        // Regenerate the first question
        generateQuestion();
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("QuizScore5", score);
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
            uiProfile.SetQuiz5Score();
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
        QnA5.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA5.RemoveAt(currentQuestion);
        generateQuestion();
    }

    private void setAnswers()
    {
        for (int i = 0; i < options5.Length; i++)
        {
            options5[i].GetComponent<AnswerScript5>().isCorrect = false;
            options5[i].transform.GetChild(0).GetComponent<Text>().text = QnA5[currentQuestion].Answers[i];

            if (QnA5[currentQuestion].CorrectAnswer == i + 1)
            {
                options5[i].GetComponent<AnswerScript5>().isCorrect = true;
            }
        }
    }


    void generateQuestion()
    {
        if (QnA5.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA5.Count);

            QuestionTxt.text = QnA5[currentQuestion].Question;
            setAnswers();
        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }
    }
}
