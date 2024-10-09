using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager8 : MonoBehaviour
{
    public List<QuestionAndAnswer8> QnA8;
    private List<QuestionAndAnswer8> originalQnA8;
    public GameObject[] options8;
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
        totalQuestions = QnA8.Count;
        originalQnA8 = new List<QuestionAndAnswer8>(QnA8);
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
        string currentQuiz8 = quizData.QuizNumber8;
        Debug.Log($"Raw QuizScore_8: {currentQuiz8}");

        if (!string.IsNullOrEmpty(currentQuiz8))
        {
            if (int.TryParse(currentQuiz8, out int savedScore8))
            {
                score = savedScore8;
                Debug.Log($"Parsed score: {score}");
                ShowScorePanel();
            }
            else
            {
                string[] scoreParts = currentQuiz8.Split('/');
                if (scoreParts.Length == 2 && int.TryParse(scoreParts[0], out savedScore8))
                {
                    score = savedScore8;
                    Debug.Log($"Parsed score from 'score/total' format: {score}");
                    ShowScorePanel();
                }
                else
                {
                    Debug.LogError($"Failed to parse QuizScore_7: {currentQuiz8}");
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
        QnA8 = new List<QuestionAndAnswer8>(originalQnA8);
        totalQuestions = QnA8.Count;
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
            UserProfile.Instance.SetQuiz8Score();
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