using TMPro;
using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText; // Reference to the Text component displaying the timer
    private float elapsedTime = 0f;
    private bool timerRunning = false;

    private void Start()
    {
        timerText.text = "00:00";
    }

    public void StartTimer()
    {
        timerRunning = true;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timerRunning)
        {
            yield return new WaitForSeconds(1f); // Update every second
            elapsedTime += 1f;
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetElapsedTime() // Add this method to get the elapsed time
    {
        return elapsedTime;
    }
}
