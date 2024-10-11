using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;   // Reference to the VideoPlayer component
    public GameObject introPanel;     // Reference to the panel that holds the video

    private float[] playbackSpeeds = { 1f, 2f, 3f}; // Array of playback speeds
    private int currentSpeedIndex = 0; // Tracks the current playback speed
    private bool isFastForwarding = false; // Tracks if the fast forward is active

    private void Start()
    {
        videoPlayer.playbackSpeed = playbackSpeeds[currentSpeedIndex]; // Set initial playback speed
        PlayIntroVideo();
        MouseManager.Instance.EnableMouse();
    }

    private void Update()
    {
        // Check if spacebar is held down
        if (Input.GetKey(KeyCode.Space))
        {
            // If not already fast forwarding, start fast forwarding
            if (!isFastForwarding)
            {
                isFastForwarding = true;
                IncreasePlaybackSpeed();
            }
        }
        else
        {
            // If spacebar is released, reset playback speed
            if (isFastForwarding)
            {
                isFastForwarding = false;
                ResetPlaybackSpeed();
            }
        }
    }

    public void PlayIntroVideo()
    {
        if (videoPlayer != null && introPanel != null)
        {
            introPanel.SetActive(true);   // Make sure the intro panel is active
            videoPlayer.Play();           // Play the video
            videoPlayer.loopPointReached += OnIntroVideoEnd; // Subscribe to event when video finishes
        }
    }

    private void OnIntroVideoEnd(VideoPlayer vp)
    {
        introPanel.SetActive(false); // Disable the intro panel after the video ends
        videoPlayer.loopPointReached -= OnIntroVideoEnd; // Unsubscribe from the event
    }

    // Method to increase playback speed
    private void IncreasePlaybackSpeed()
    {
        if (currentSpeedIndex < playbackSpeeds.Length - 1)
        {
            currentSpeedIndex++; // Move to the next speed level
            videoPlayer.playbackSpeed = playbackSpeeds[currentSpeedIndex]; // Apply the new speed
            Debug.Log($"Playback speed increased to {playbackSpeeds[currentSpeedIndex]}x");
        }
    }

    // Method to reset playback speed to normal
    private void ResetPlaybackSpeed()
    {
        currentSpeedIndex = 0; // Reset to normal speed
        videoPlayer.playbackSpeed = playbackSpeeds[currentSpeedIndex]; // Apply normal speed
        Debug.Log("Playback speed reset to normal.");
    }
}
