using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        totalQuestions = QnA.Count;
        originalQnA = new List<QuestionAndAnswer>(QnA);
    }

    private void OnEnable()
    {
        UserProfile.OnQuizzesUpdated.AddListener(UpdateQuizState);

        if (UserProfile.Instance != null)
        {
            UpdateQuizState(UserProfile.Instance.quizData);
        }
    }

    private void OnDisable()
    {
        UserProfile.OnQuizzesUpdated.RemoveListener(UpdateQuizState);
    }

    private void UpdateQuizState(QuizzesScores quizData)
    {
        string currentQuiz = quizData.QuizNumber1;
        Debug.Log($"Raw QuizScore_1: {currentQuiz}");

        if (!string.IsNullOrEmpty(currentQuiz))
        {
            if (int.TryParse(currentQuiz, out int savedScore))
            {
                score = savedScore;
                Debug.Log($"Parsed score: {score}");
                ShowScorePanel();
            }
            else
            {
                string[] scoreParts = currentQuiz.Split('/');
                if (scoreParts.Length == 2 && int.TryParse(scoreParts[0], out savedScore))
                {
                    score = savedScore;
                    Debug.Log($"Parsed score from 'score/total' format: {score}");
                    ShowScorePanel();
                }
                else
                {
                    Debug.LogError($"Failed to parse QuizScore_1: {currentQuiz}");
                    StartQuizNormally();
                }
            }
        }
        else if (isRetry)
        {
            Debug.Log("Retry detected, starting quiz normally");
            StartQuizNormally();
        }
        else
        {
            Debug.Log("No saved score and not a retry, starting quiz normally");
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