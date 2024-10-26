using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject introPanel;
    public RawImage videoScreen;
    public VideoClip videoClip;       // Direct reference to video asset
    private float[] playbackSpeeds = { 1f, 2f, 3f };
    private int currentSpeedIndex = 0;
    private bool isFastForwarding = false;

    private void Awake()
    {
        // Ensure we have a RenderTexture target
        if (videoPlayer.targetTexture == null)
        {
            RenderTexture rt = new RenderTexture(1920, 1080, 24);
            videoPlayer.targetTexture = rt;
            if (videoScreen != null)
            {
                videoScreen.texture = rt;
            }
        }

        // Method 1: Direct VideoClip Reference
        if (videoClip != null)
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoClip;
        }
        // Method 2: Load from Resources folder
        else
        {
            VideoClip clipFromResources = Resources.Load<VideoClip>("Videos/YourVideoName") as VideoClip;
            if (clipFromResources != null)
            {
                videoPlayer.source = VideoSource.VideoClip;
                videoPlayer.clip = clipFromResources;
            }
            else
            {
                Debug.LogError("Failed to load video from Resources!");
            }
        }

        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.skipOnDrop = true;
    }

    private void Start()
    {
        if (videoPlayer == null || introPanel == null || videoScreen == null)
        {
            Debug.LogError("Missing required references in IntroVideo!");
            return;
        }

        videoPlayer.playbackSpeed = playbackSpeeds[currentSpeedIndex];
        StartCoroutine(PrepareAndPlayVideo());
        MouseManager.Instance.EnableMouse();
    }

    private IEnumerator PrepareAndPlayVideo()
    {
        Debug.Log("Starting video preparation...");

        // Start preparing the video
        videoPlayer.Prepare();

        // Wait until the video is prepared
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing video...");
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Video preparation completed!");
        PlayIntroVideo();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isFastForwarding)
            {
                isFastForwarding = true;
                IncreasePlaybackSpeed();
            }
        }
        else
        {
            if (isFastForwarding)
            {
                isFastForwarding = false;
                ResetPlaybackSpeed();
            }
        }
    }

    public void PlayIntroVideo()
    {
        if (!videoPlayer.isPrepared)
        {
            Debug.LogWarning("Attempting to play unprepared video!");
            return;
        }

        introPanel.SetActive(true);
        videoScreen.gameObject.SetActive(true);
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnIntroVideoEnd;

        Debug.Log("Video playback started");
    }

    private void OnIntroVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video playback completed");
        introPanel.SetActive(false);
        videoScreen.gameObject.SetActive(false);
        videoPlayer.loopPointReached -= OnIntroVideoEnd;
    }

    private void IncreasePlaybackSpeed()
    {
        if (currentSpeedIndex < playbackSpeeds.Length - 1)
        {
            currentSpeedIndex++;
            videoPlayer.playbackSpeed = playbackSpeeds[currentSpeedIndex];
            Debug.Log($"Playback speed increased to {playbackSpeeds[currentSpeedIndex]}x");
        }
    }

    private void ResetPlaybackSpeed()
    {
        currentSpeedIndex = 0;
        videoPlayer.playbackSpeed = playbackSpeeds[currentSpeedIndex];
        Debug.Log("Playback speed reset to normal.");
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnIntroVideoEnd;
        }
    }
}