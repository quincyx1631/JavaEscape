using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager7 : MonoBehaviour
{
    public List<QuestionAndAnswer7> QnA7;
    private List<QuestionAndAnswer7> originalQnA7;
    public GameObject[] options7;
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
        totalQuestions = QnA7.Count;
        originalQnA7 = new List<QuestionAndAnswer7>(QnA7);
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
        string currentQuiz7 = profileData.QuizScore_7;
        Debug.Log($"Raw QuizScore_7: {currentQuiz7}");

        if (!string.IsNullOrEmpty(currentQuiz7))
        {
            if (int.TryParse(currentQuiz7, out int savedScore7))
            {
                score = savedScore7;
                Debug.Log($"Parsed score: {score}");
                ShowScorePanel();
            }
            else
            {
                string[] scoreParts = currentQuiz7.Split('/');
                if (scoreParts.Length == 2 && int.TryParse(scoreParts[0], out savedScore7))
                {
                    score = savedScore7;
                    Debug.Log($"Parsed score from 'score/total' format: {score}");
                    ShowScorePanel();
                }
                else
                {
                    Debug.LogError($"Failed to parse QuizScore_7: {currentQuiz7}");
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
        QnA7 = new List<QuestionAndAnswer7>(originalQnA7);
        totalQuestions = QnA7.Count;
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