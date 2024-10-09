using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager2 : MonoBehaviour
{
    public List<QuestionAndAnswer2> QnA2;
    private List<QuestionAndAnswer2> originalQnA2;
    public GameObject[] options2;
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
        totalQuestions = QnA2.Count;
        originalQnA2 = new List<QuestionAndAnswer2>(QnA2);
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
        string currentQuiz2 = quizData.QuizNumber2;
        Debug.Log($"Raw QuizScore_2: {currentQuiz2}"); 

        if (!string.IsNullOrEmpty(currentQuiz2))
        {
            if (int.TryParse(currentQuiz2, out int savedScore2))
            {
                score = savedScore2;
                Debug.Log($"Parsed score: {score}");
                ShowScorePanel();
            }
            else
            {
                string[] scoreParts = currentQuiz2.Split('/');
                if (scoreParts.Length == 2 && int.TryParse(scoreParts[0], out savedScore2))
                {
                    score = savedScore2;
                    Debug.Log($"Parsed score from 'score/total' format: {score}");
                    ShowScorePanel();
                }
                else
                {
                    Debug.LogError($"Failed to parse QuizScore_2: {currentQuiz2}");
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
        QnA2 = new List<QuestionAndAnswer2>(originalQnA2);
        totalQuestions = QnA2.Count;
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
            UserProfile.Instance.SetQuiz2Score();
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