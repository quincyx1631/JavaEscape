using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager5 : MonoBehaviour
{
    public List<QuestionAndAnswer5> QnA5;
    private List<QuestionAndAnswer5> originalQnA5;
    public GameObject[] options5;
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
        totalQuestions = QnA5.Count;
        originalQnA5 = new List<QuestionAndAnswer5>(QnA5);
    }

    private void OnEnable()
    {
        UserProfile.OnProfileDataUpdated.AddListener(UpdateQuizState);

        if (UserProfile.Instance != null)
        {
            UpdateQuizState(UserProfile.Instance.profileData);
        }
    }

    private void OnDisable()
    {
        UserProfile.OnProfileDataUpdated.RemoveListener(UpdateQuizState);
    }

    private void UpdateQuizState(ProfileData profileData)
    {
        string currentQuiz5 = profileData.QuizScore_5;
        Debug.Log($"Raw QuizScore_5: {currentQuiz5}"); 

        if (!string.IsNullOrEmpty(currentQuiz5))
        {
            if (int.TryParse(currentQuiz5, out int savedScore5))
            {
                score = savedScore5;
                Debug.Log($"Parsed score: {score}");
                ShowScorePanel();
            }
            else
            {
                string[] scoreParts = currentQuiz5.Split('/');
                if (scoreParts.Length == 2 && int.TryParse(scoreParts[0], out savedScore5))
                {
                    score = savedScore5;
                    Debug.Log($"Parsed score from 'score/total' format: {score}");
                    ShowScorePanel();
                }
                else
                {
                    Debug.LogError($"Failed to parse QuizScore_5: {currentQuiz5}");
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
        QnA5 = new List<QuestionAndAnswer5>(originalQnA5);
        totalQuestions = QnA5.Count;
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