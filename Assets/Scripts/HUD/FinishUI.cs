using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishUI : MonoBehaviour
{
    public static FinishUI Instance;

    public TMP_Text finalTimeText; // Reference to the TMP_Text component for displaying the final time
    public TMP_Text finalScoreText; // Reference to the TMP_Text component for displaying the final score
    public Animator finishUIAnimator; // Reference to the Animator component for the UI animation
    public BlurEffect blurEffect; // Reference to the BlurEffect component
    private float fadeDuration = 1f; // Duration of the fade-in effect

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

        // Ensure the score text starts with an opacity of 0
        finalScoreText.color = new Color(finalScoreText.color.r, finalScoreText.color.g, finalScoreText.color.b, 0);
        finalScoreText.gameObject.SetActive(true); // Keep it active but invisible initially
    }

    public void DisplayFinalTime(float finalTime)
    {
        PlayerControlManager.Instance.DisablePlayerControls();

        // Convert time to minutes and seconds
        int minutes = Mathf.FloorToInt(finalTime / 60);
        int seconds = Mathf.FloorToInt(finalTime % 60);
        finalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Enable the blur effect
        blurEffect.EnableBlur();

        // Play the UI animation
        finishUIAnimator.SetTrigger("ShowFinishUI");

        // Enable the mouse cursor
        MouseManager.Instance.EnableMouse();
    }

    public void RevealScore(float remainingTime)
    {
        float score = Timer.Instance.GetRealTimeScore(); // Get the final real-time score
        finalScoreText.text = "Score: " + score.ToString("0"); // Display the score
        StartCoroutine(FadeInScore()); // Start the fade-in effect
    }

    private IEnumerator FadeInScore()
    {
        Color textColor = finalScoreText.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            finalScoreText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null; // Wait for the next frame
        }

        finalScoreText.color = new Color(textColor.r, textColor.g, textColor.b, 1); // Ensure it ends at full opacity
    }

    public void LoadMainMenuNext()
    {
        SceneManager.LoadScene("Main Menu Final");
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("You're not in the correct scene");
        }
    }
}
