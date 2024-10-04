using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager4 : MonoBehaviour
{
    public List<QuestionAndAnswer4> QnA4;
    private List<QuestionAndAnswer4> originalQnA4;
    public GameObject[] options4;
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
        totalQuestions = QnA4.Count;
        originalQnA4 = new List<QuestionAndAnswer4>(QnA4);
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
        string currentQuiz4 = profileData.QuizScore_4;
        Debug.Log($"Raw QuizScore_4: {currentQuiz4}"); 

        if (!string.IsNullOrEmpty(currentQuiz4))
        {
            if (int.TryParse(currentQuiz4, out int savedScore4))
            {
                score = savedScore4;
                Debug.Log($"Parsed score: {score}");
                ShowScorePanel();
            }
            else
            {
                string[] scoreParts = currentQuiz4.Split('/');
                if (scoreParts.Length == 2 && int.TryParse(scoreParts[0], out savedScore4))
                {
                    score = savedScore4;
                    Debug.Log($"Parsed score from 'score/total' format: {score}");
                    ShowScorePanel();
                }
                else
                {
                    Debug.LogError($"Failed to parse QuizScore_4: {currentQuiz4}");
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
        QnA4 = new List<QuestionAndAnswer4>(originalQnA4);
        totalQuestions = QnA4.Count;
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
        QnA4.RemoveAt(currentQuestion);
        generateQuestion();
    }

    public void wrong()
    {
        QnA4.RemoveAt(currentQuestion);
        generateQuestion();
    }

    private void setAnswers()
    {
        for (int i = 0; i < options4.Length; i++)
        {
            options4[i].GetComponent<AnswerScript4>().isCorrect = false;
            options4[i].transform.GetChild(0).GetComponent<Text>().text = QnA4[currentQuestion].Answers[i];
            if (QnA4[currentQuestion].CorrectAnswer == i + 1)
            {
                options4[i].GetComponent<AnswerScript4>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        if (QnA4.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA4.Count);
            QuestionTxt.text = QnA4[currentQuestion].Question;
            setAnswers();
        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }
    }
}