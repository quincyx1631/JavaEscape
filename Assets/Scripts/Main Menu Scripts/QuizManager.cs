using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//NEED TO APPLY SA IBA
public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswer> QnA;
    private List<QuestionAndAnswer> originalQnA;
    public GameObject[] options;
    public int currentQuestion;
    public Text QuestionTxt;
    public GameObject QuizPanel;
    public GameObject ScorePanel;
    public Text scoreTxt;
    int totalQuestions = 0;
    public int score = 0;
    private bool isRetry = false;

    private void Start()
    {
        totalQuestions = QnA.Count;
        originalQnA = new List<QuestionAndAnswer>(QnA);

        UserProfile.OnProfileDataUpdated.AddListener(UpdateQuizState);
        if (UserProfile.Instance != null)
        {
            UpdateQuizState(UserProfile.Instance.profileData);
        }
    }

    private void UpdateQuizState(ProfileData profileData)
    {
        string currentQuiz = profileData.QuizScore_1;

        if (!string.IsNullOrEmpty(currentQuiz))
        {
            // If there is a saved score, show the ScorePanel
            string[] scoreParts = currentQuiz.Split('/');
            if (scoreParts.Length == 2 && int.TryParse(scoreParts[0], out int savedScore))
            {
                score = savedScore;
                ShowScorePanel();
            }
            else
            {
                Debug.LogError("Failed to parse QuizScore_1");
                StartQuizNormally();
            }
        }
        else if (isRetry)
        {
            // If it's a retry, start the quiz
            StartQuizNormally();
        }
        else
        {
            // Start quiz normally if no saved score and not a retry
            StartQuizNormally();
        }
    }

    private void StartQuizNormally()
    {
        ScorePanel.SetActive(false);
        QuizPanel.SetActive(true);
        generateQuestion();
    }

    public void retry()
    {
        isRetry = true;
        score = 0;
        QnA = new List<QuestionAndAnswer>(originalQnA);
        totalQuestions = QnA.Count;
        ScorePanel.SetActive(false);
        QuizPanel.SetActive(true);
        generateQuestion();
    }

    void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        ShowScorePanel();
        yield return new WaitForSecondsRealtime(1f);

        if (UserProfile.Instance != null)
        {
            Debug.Log("UserProfile found. Setting quiz score...");
            Debug.Log("Quiz score set successfully.");
            UserProfile.Instance.SetQuizScore();
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
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    private void setAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];
            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestion].Question;
            setAnswers();
        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }
    }
}