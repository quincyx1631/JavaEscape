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
    public GameObject scoreButton; // Reference to the button that reveals the score
    private float fadeDuration = 1f; // Duration of the fade-in effect

    // Add references for the other UI elements you want to disable
    public GameObject[] uiElementsToDisable; // Array of UI elements to be disabled

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

        // Ensure the score button is active and the score text is invisible initially
        scoreButton.SetActive(true);
    }

    public void DisplayFinalTime(float finalTime)
    {
        // Disable the other UI elements
        DisableUIElements();
        InputManager.Instance.BlockInput();
        // Activate the finish UI if it's not already active
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

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

        // Ensure the score text remains hidden (opacity = 0) until the button is pressed
        finalScoreText.color = new Color(finalScoreText.color.r, finalScoreText.color.g, finalScoreText.color.b, 0);
    }

    public void RevealScore()
    {
        float score = Timer.Instance.GetRealTimeScore(); // Get the final real-time score
        finalScoreText.text = "Score: " + score.ToString("0"); // Update the score text

        // Start the fade-in effect to reveal the score
        StartCoroutine(FadeInScore());
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

    // New method to disable specific UI elements
    private void DisableUIElements()
    {
        foreach (GameObject element in uiElementsToDisable)
        {
            if (element != null)
            {
                element.SetActive(false); // Disable the UI element
            }
        }
    }

    // Optionally, you can add a method to re-enable the UI elements
    public void ReEnableUIElements()
    {
        foreach (GameObject element in uiElementsToDisable)
        {
            if (element != null)
            {
                element.SetActive(true); // Re-enable the UI element
            }
        }
    }

    public void LoadMainMenuNext()
    {
        InputManager.Instance.UnblockInput();
        SceneManager.LoadScene("Main Menu Final");
    }

    public void NextLevel()
    {
        InputManager.Instance.UnblockInput();
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
