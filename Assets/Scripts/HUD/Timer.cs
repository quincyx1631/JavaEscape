using TMPro;
using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText; // Reference to the Text component displaying the timer
    private float startTime;
    private bool timerRunning = false;

    private void Start()
    {
        timerText.text = "00:00";
    }

    public void StartTimer()
    {
        startTime = Time.time;
        timerRunning = true;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timerRunning)
        {
            float elapsedTime = Time.time - startTime;
            UpdateTimerText(elapsedTime);
            yield return null; // Update every frame
        }
    }

    private void UpdateTimerText(float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetElapsedTime()
    {
        return Time.time - startTime;
    }
}
