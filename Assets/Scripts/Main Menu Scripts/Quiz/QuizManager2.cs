using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager2 : MonoBehaviour
{
    public List<QuestionAndAnswer2> QnA2;
    private List<QuestionAndAnswer2> originalQnA2; // Original list to reset questions
    public GameObject[] options2;
    public int currentQuestion;

    public Text QuestionTxt;

    public GameObject QuizPanel;
    public GameObject ScorePanel;

    public Text scoreTxt;
    int totalQuestions = 0;
    public int score = 0;

    private void Start()
    {
        totalQuestions = QnA2.Count;
        PlayerPrefs.DeleteKey("QuizRetry2");
        PlayerPrefs.DeleteKey("QuizScore2");
        // Create a copy of the original QnA2 list
        originalQnA2 = new List<QuestionAndAnswer2>(QnA2);

        // Check if there is a retry flag
        if (PlayerPrefs.HasKey("QuizRetry2"))
        {
            PlayerPrefs.DeleteKey("QuizRetry2"); // Clear the retry flag
            ScorePanel.SetActive(false);
            generateQuestion();
        }
        else if (PlayerPrefs.HasKey("QuizScore2"))
        {
            // If there is a saved score, show the ScorePanel
            score = PlayerPrefs.GetInt("QuizScore2");
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
        PlayerPrefs.SetInt("QuizRetry2", 1);
        PlayerPrefs.Save();

        // Reset the score and the question list
        score = 0;
        QnA2 = new List<QuestionAndAnswer2>(originalQnA2); // Reset QnA2 to the original list
        totalQuestions = QnA2.Count;

        // Reset the UI elements
        ScorePanel.SetActive(false);
        QuizPanel.SetActive(true);

        // Regenerate the first question
        generateQuestion();
    }

    void GameOver()
    {
        // Save the score to PlayerPrefs
        PlayerPrefs.SetInt("QuizScore2", score);
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
            uiProfile.SetQuiz2Score();
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
        QnA2.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA2.RemoveAt(currentQuestion);
        generateQuestion();
    }

    private void setAnswers()
    {
        for (int i = 0; i < options2.Length; i++)
        {
            options2[i].GetComponent<AnswerScript2>().isCorrect = false;
            options2[i].transform.GetChild(0).GetComponent<Text>().text = QnA2[currentQuestion].Answers[i];

            if (QnA2[currentQuestion].CorrectAnswer == i + 1)
            {
                options2[i].GetComponent<AnswerScript2>().isCorrect = true;
            }
        }
    }


    void generateQuestion()
    {
        if (QnA2.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA2.Count);

            QuestionTxt.text = QnA2[currentQuestion].Question;
            setAnswers();
        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }
    }
}
