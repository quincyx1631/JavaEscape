using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager3 : MonoBehaviour
{
    public List<QuestionAndAnswer3> QnA3;
    private List<QuestionAndAnswer3> originalQnA3;
    public GameObject[] options3;
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
        totalQuestions = QnA3.Count;
        originalQnA3 = new List<QuestionAndAnswer3>(QnA3);
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
        string currentQuiz3 = profileData.QuizScore_3;
        Debug.Log($"Raw QuizScore_3: {currentQuiz3}"); 

        if (!string.IsNullOrEmpty(currentQuiz3))
        {
            if (int.TryParse(currentQuiz3, out int savedScore3))
            {
                score = savedScore3;
                Debug.Log($"Parsed score: {score}");
                ShowScorePanel();
            }
            else
            {
                string[] scoreParts = currentQuiz3.Split('/');
                if (scoreParts.Length == 2 && int.TryParse(scoreParts[0], out savedScore3))
                {
                    score = savedScore3;
                    Debug.Log($"Parsed score from 'score/total' format: {score}");
                    ShowScorePanel();
                }
                else
                {
                    Debug.LogError($"Failed to parse QuizScore_3: {currentQuiz3}");
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
        QnA3 = new List<QuestionAndAnswer3>(originalQnA3);
        totalQuestions = QnA3.Count;
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