using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class FadeInIntro : MonoBehaviour
{
    public bool hasVideoIntro = false;
    public VideoPlayer videoPlayer;
    public GameObject videoPanel;
    public Image blackScreen;
    public TMP_Text[] introTexts;
    public Dialogue dialogueSystem;
    public string introAudioName;
    public string outroAudioName;

    public float fadeDuration = 2.0f;
    public float textVisibleDuration = 1.5f;
    public float textFadeDuration = 1.5f;
    public float audioFadeDuration = 1.5f;

    // New video-related variables
    public float videoFadeInDuration = 1.0f;
    public float videoFadeOutDuration = 1.0f;
    public Image videoFadeOverlay; // Optional overlay for smooth video transitions

   public bool isInFadeInSequence = false;

    public GameObject[] uiElementsToHide;

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return null;

        Debug.Log("Starting FadeInIntro, hiding UI elements.");

        foreach (GameObject uiElement in uiElementsToHide)
        {
            if (uiElement != null)
            {
                Debug.Log($"Hiding UI element: {uiElement.name}");
                uiElement.SetActive(false);
            }
            else
            {
                Debug.LogWarning("UI element in uiElementsToHide array is null!");
            }
        }

        PlayerControlManager.Instance.DisablePlayerControls();

        isInFadeInSequence = true;
        if (hasVideoIntro && videoPlayer != null)
        {
            StartCoroutine(PlayVideoIntro());
        }
        else
        {
            StartCoroutine(FadeInSequence());
        }
        isInFadeInSequence = false;
    }

    private IEnumerator PlayVideoIntro()
    {
        // Prepare video and overlay
        videoPanel.SetActive(true);
        if (videoFadeOverlay != null)
        {
            videoFadeOverlay.gameObject.SetActive(true);
            videoFadeOverlay.color = new Color(0, 0, 0, 1);
        }

        // Prepare the video
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Start playing the video
        videoPlayer.Play();

        // Fade in the video
        float elapsedTime = 0f;
        while (elapsedTime < videoFadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - (elapsedTime / videoFadeInDuration);

            if (videoFadeOverlay != null)
            {
                videoFadeOverlay.color = new Color(0, 0, 0, alpha);
            }
            yield return null;
        }

        // Wait for video to complete
        while (videoPlayer.isPlaying)
        {
            // Check if we're near the end of the video
            if (videoPlayer.frame > 0 && videoPlayer.frame >= (long)(videoPlayer.frameCount - 30)) // 30 frames before end
            {
                break; // Exit early to prevent any visual hiccup
            }
            yield return null;
        }

        // Fade out the video
        elapsedTime = 0f;
        while (elapsedTime < videoFadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / videoFadeOutDuration;

            if (videoFadeOverlay != null)
            {
                videoFadeOverlay.color = new Color(0, 0, 0, alpha);
            }
            yield return null;
        }

        // Clean up
        videoPlayer.Stop();
        videoPanel.SetActive(false);
        if (videoFadeOverlay != null)
        {
            videoFadeOverlay.gameObject.SetActive(false);
        }

        // Proceed with the regular fade sequence
        StartCoroutine(FadeInSequence());
    }

    public IEnumerator FadeInSequence()
    {
        isInFadeInSequence = true;

        // Ensure the dialogue system is inactive initially
        if (dialogueSystem != null)
        {
            dialogueSystem.gameObject.SetActive(false);
        }

        // Phase 1: Fade out the black screen
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Ensure the black screen is fully transparent
        blackScreen.color = new Color(0, 0, 0, 0);

        // Wait before starting the text fade-in
        yield return new WaitForSeconds(0.5f); // Delay before showing the text

        // Play the intro sound effect
        if (!string.IsNullOrEmpty(introAudioName))
        {
            AudioManager.Instance.Play(introAudioName);
        }

        // Phase 2: Fade in all intro texts together
        foreach (TMP_Text introText in introTexts)
        {
            Color textColor = introText.color;
            textColor.a = 0;
            introText.color = textColor;
            introText.gameObject.SetActive(true);
        }

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            foreach (TMP_Text introText in introTexts)
            {
                Color textColor = introText.color;
                textColor.a = alpha;
                introText.color = textColor;
            }
            yield return null;
        }

        // Ensure all texts are fully visible
        foreach (TMP_Text introText in introTexts)
        {
            Color textColor = introText.color;
            textColor.a = 1;
            introText.color = textColor;
        }

        // Wait for the specified duration with the texts fully visible
        yield return new WaitForSeconds(textVisibleDuration);

        // Start fading out the sound effect
        if (!string.IsNullOrEmpty(outroAudioName))
        {
            AudioManager.Instance.FadeOutAndStop(outroAudioName, audioFadeDuration);
        }

        // Fade out all intro texts together
        elapsedTime = 0f;
        while (elapsedTime < textFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / textFadeDuration));
            foreach (TMP_Text introText in introTexts)
            {
                Color textColor = introText.color;
                textColor.a = alpha;
                introText.color = textColor;
            }
            yield return null;
        }

        // Ensure all texts are fully invisible
        foreach (TMP_Text introText in introTexts)
        {
            Color textColor = introText.color;
            textColor.a = 0;
            introText.color = textColor;
            introText.gameObject.SetActive(false);
        }

        // Hide the black screen after the fade-out
        blackScreen.gameObject.SetActive(false);

        // Activate the dialogue system after all texts fade out
        if (dialogueSystem != null)
        {
            dialogueSystem.gameObject.SetActive(true);
            dialogueSystem.StartDialogue(); // Ensure StartDialogue method exists and is accessible
        }
        isInFadeInSequence = false;
    }

    // Method to show the hidden UI elements
    public void ShowUIElements()
    {
        foreach (GameObject uiElement in uiElementsToHide)
        {
            if (uiElement != null)
            {
                Debug.Log($"Showing UI element: {uiElement.name}");
                uiElement.SetActive(true);
            }
            else
            {
                Debug.LogWarning("UI element in uiElementsToHide array is null!");
            }
        }
    }
}
