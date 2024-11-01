using UnityEngine;
using UnityEngine.Video;

public class TVTutsPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public GameObject tvObject;     // The object that holds the TV

    // Function to play the video
    public void PlayTVVideo()
    {
        if (videoPlayer != null && tvObject != null)
        {
            tvObject.SetActive(true);  // Make sure the TV object is active
            videoPlayer.Play();        // Play the video
            videoPlayer.loopPointReached += OnVideoEnd; // Subscribe to event when video finishes
        }
    }

    // Called when the video finishes
    private void OnVideoEnd(VideoPlayer vp)
    {
        tvObject.SetActive(false); // Disable the TV object after the video ends
        videoPlayer.loopPointReached -= OnVideoEnd; // Unsubscribe from the event
    }
}
