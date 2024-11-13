using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndRedirect : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign your VideoPlayer in the Inspector

    private void Start()
    {
        // Listen for when the video has finished
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Load the main menu after the video finishes
        SceneManager.LoadScene("Main Menu Final"); // Replace "MainMenu" with your actual main menu scene name
    }
}
