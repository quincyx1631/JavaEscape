using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager6 : MonoBehaviour
{
    public List<QuestionAndAnswer6> QnA6;
    private List<QuestionAndAnswer6> originalQnA6;
    public GameObject[] options6;
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
        totalQuestions = QnA6.Count;
        originalQnA6 = new List<QuestionAndAnswer6>(QnA6);
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
        string currentQuiz6 = quizData.QuizNumber6;
        Debug.Log($"Raw QuizScore_6: {currentQuiz6}");

        if (!string.IsNullOrEmpty(currentQuiz6))
        {
            if (int.TryParse(currentQuiz6, out int savedScore6))
            {
                score = savedScore6;
                Debug.Log($"Parsed score: {score}");
                ShowScorePanel();
            }
            else
            {
                string[] scoreParts = currentQuiz6.Split('/');
                if (scoreParts.Length == 2 && int.TryParse(scoreParts[0], out savedScore6))
                {
                    score = savedScore6;
                    Debug.Log($"Parsed score from 'score/total' format: {score}");
                    ShowScorePanel();
                }
                else
                {
                    Debug.LogError($"Failed to parse QuizScore_6: {currentQuiz6}");
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
        QnA6 = new List<QuestionAndAnswer6>(originalQnA6);
        totalQuestions = QnA6.Count;
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
            UserProfile.Instance.SetQuiz6Score();
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