using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinishUI : MonoBehaviour
{
    public static FinishUI Instance;

    public TMP_Text finalTimeText; // Reference to the TMP_Text component for displaying the final time
    public Animator finishUIAnimator; // Reference to the Animator component for the UI animation
    public BlurEffect blurEffect; // Reference to the BlurEffect component

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

    public void LoadMainMenuNext()
    {
        SceneManager.LoadScene("Main Menu Final");
    }

    public void nextLevel()
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
