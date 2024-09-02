using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class QuizManager7 : MonoBehaviour
{
    public List<QuestionAndAnswer7> QnA7;
    private List<QuestionAndAnswer7> originalQnA7; // Original list to reset questions
    public GameObject[] options7;
    public int currentQuestion;

    public Text QuestionTxt;

    public GameObject QuizPanel;
    public GameObject ScorePanel;

    public Text scoreTxt;
    int totalQuestions = 0;
    public int score = 0;

    private void Start()
    {
        totalQuestions = QnA7.Count;
        PlayerPrefs.DeleteKey("QuizRetry7");
        PlayerPrefs.DeleteKey("QuizScore7");
        // Create a copy of the original QnA7 list
        originalQnA7 = new List<QuestionAndAnswer7>(QnA7);

        // Check if there is a retry flag
        if (PlayerPrefs.HasKey("QuizRetry7"))
        {
            PlayerPrefs.DeleteKey("QuizRetry7"); // Clear the retry flag
            ScorePanel.SetActive(false);
            generateQuestion();
        }
        else if (PlayerPrefs.HasKey("QuizScore7"))
        {
            // If there is a saved score, show the ScorePanel
            score = PlayerPrefs.GetInt("QuizScore7");
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
        PlayerPrefs.SetInt("QuizRetry7", 1);
        PlayerPrefs.Save();

        // Reset the score and the question list
        score = 0;
        QnA7 = new List<QuestionAndAnswer7>(originalQnA7); // Reset QnA7 to the original list
        totalQuestions = QnA7.Count;

        // Reset the UI elements
        ScorePanel.SetActive(false);
        QuizPanel.SetActive(true);

        // Regenerate the first question
        generateQuestion();
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("QuizScore7", score);
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
            uiProfile.SetQuiz7Score();
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
        QnA7.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA7.RemoveAt(currentQuestion);
        generateQuestion();
    }

    private void setAnswers()
    {
        for (int i = 0; i < options7.Length; i++)
        {
            options7[i].GetComponent<AnswerScript7>().isCorrect = false;
            options7[i].transform.GetChild(0).GetComponent<Text>().text = QnA7[currentQuestion].Answers[i];

            if (QnA7[currentQuestion].CorrectAnswer == i + 1)
            {
                options7[i].GetComponent<AnswerScript7>().isCorrect = true;
            }
        }
    }


    void generateQuestion()
    {
        if (QnA7.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA7.Count);

            QuestionTxt.text = QnA7[currentQuestion].Question;
            setAnswers();
        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }
    }
}
