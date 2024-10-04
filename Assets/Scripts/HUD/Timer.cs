using TMPro;
using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    public static Timer Instance; // Singleton instance

    public TMP_Text timerText; // Reference to the Text component displaying the timer
    public TMP_Text realTimeScoreText; // Reference to TMP_Text component for real-time score display

    [Header("Timer Settings")]
    public float countdownTime = 3600f; // Set this to 60 minutes (3600 seconds)

    [Header("Scoring Settings")]
    public float perfectTime = 1800f; // 30 minutes remaining for perfect score
    public float perfectScore = 50f; // Perfect score is 50
    public float lowestScore = 30f; // Minimum possible score for completing the game
    public float minTimeForScore = 0f; // Minimum time to get a score (0 seconds)

    private float remainingTime;
    private bool timerRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        remainingTime = countdownTime;
        timerText.text = "60:00"; // Display initial time
    }

    public void StartTimer()
    {
        timerRunning = true;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timerRunning && remainingTime > 0)
        {
            UpdateTimerText(remainingTime);
            UpdateRealTimeScore();

            yield return new WaitForSeconds(1f); // Wait for one second
            remainingTime -= 1f; // Decrease remaining time by 1 second
        }

        // Stop the timer when it reaches 0
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            UpdateTimerText(remainingTime); // Update the final text to 00:00
            timerRunning = false; // Stop the timer

            float elapsedTime = countdownTime - remainingTime; // Calculate elapsed time
            FinishUI.Instance.DisplayFinalTime(elapsedTime); // Pass final time
            FinishUI.Instance.RevealScore(remainingTime); // Pass remaining time to calculate score
        }
    }


    private void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Update the real-time score based on remaining time
    private void UpdateRealTimeScore()
    {
        float elapsedTime = countdownTime - remainingTime;
        float realTimeScore = CalculateRealTimeScore(elapsedTime);

        // Update real-time score text
        realTimeScoreText.text = "Score: " + Mathf.FloorToInt(realTimeScore).ToString();
    }

    // Scoring system based on the remaining time and elapsed time
    private float CalculateRealTimeScore(float elapsedTime)
    {
        // If remaining time is greater than or equal to the perfect time, return the perfect score
        if (remainingTime >= perfectTime)
        {
            return perfectScore;
        }
        // If remaining time is between 0 and the perfect time, calculate the score linearly between perfect and lowest score
        else if (remainingTime > 0)
        {
            // Calculate the range for scoring
            float timeSincePerfect = perfectTime - remainingTime;
            float maxTimeRange = perfectTime; // Total time to decrease from perfect to 0
            float scoreRange = perfectScore - lowestScore; // Range of scores between perfect and lowest

            // Linear interpolation between perfect and lowest score
            float scaledScore = perfectScore - (scoreRange * (timeSincePerfect / maxTimeRange));

            // Return the clamped score (ensures it doesn't go below lowestScore or above perfectScore)
            return Mathf.Clamp(scaledScore, lowestScore, perfectScore);
        }
        // If no time is left, return 0
        else
        {
            return 0;
        }
    }


    // New method to return the real-time score
    public float GetRealTimeScore()
    {
        return CalculateRealTimeScore(countdownTime - remainingTime);
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetElapsedTime()
    {
        return countdownTime - remainingTime; // Return elapsed time
    }
}
