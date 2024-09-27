using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager3 : MonoBehaviour
{
    public List<QuestionAndAnswer3> QnA3;
    private List<QuestionAndAnswer3> originalQnA3; // Original list to reset questions
    public GameObject[] options3;
    public int currentQuestion;

    public Text QuestionTxt;

    public GameObject QuizPanel;
    public GameObject ScorePanel;

    public Text scoreTxt;
    int totalQuestions = 0;
    public int score = 0;

    private void Start()
    {
        totalQuestions = QnA3.Count;
        PlayerPrefs.DeleteKey("QuizRetry3");
        PlayerPrefs.DeleteKey("QuizScore3");
        // Create a copy of the original QnA3 list
        originalQnA3 = new List<QuestionAndAnswer3>(QnA3);

        // Check if there is a retry flag
        if (PlayerPrefs.HasKey("QuizRetry3"))
        {
            PlayerPrefs.DeleteKey("QuizRetry3"); // Clear the retry flag
            ScorePanel.SetActive(false);
            generateQuestion();
        }
        else if (PlayerPrefs.HasKey("QuizScore3"))
        {
            // If there is a saved score, show the ScorePanel
            score = PlayerPrefs.GetInt("QuizScore3");
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
        PlayerPrefs.SetInt("QuizRetry3", 1);
        PlayerPrefs.Save();

        // Reset the score and the question list
        score = 0;
        QnA3 = new List<QuestionAndAnswer3>(originalQnA3); // Reset QnA3 to the original list
        totalQuestions = QnA3.Count;

        // Reset the UI elements
        ScorePanel.SetActive(false);
        QuizPanel.SetActive(true);

        // Regenerate the first question
        generateQuestion();
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("QuizScore3", score);
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
            uiProfile.SetQuiz3Score();
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
        QnA3.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA3.RemoveAt(currentQuestion);
        generateQuestion();
    }

    private void setAnswers()
    {
        for (int i = 0; i < options3.Length; i++)
        {
            options3[i].GetComponent<AnswerScript3>().isCorrect = false;
            options3[i].transform.GetChild(0).GetComponent<Text>().text = QnA3[currentQuestion].Answers[i];

            if (QnA3[currentQuestion].CorrectAnswer == i + 1)
            {
                options3[i].GetComponent<AnswerScript3>().isCorrect = true;
            }
        }
    }


    void generateQuestion()
    {
        if (QnA3.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA3.Count);

            QuestionTxt.text = QnA3[currentQuestion].Question;
            setAnswers();
        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }
    }
}
