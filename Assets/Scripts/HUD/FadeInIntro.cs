using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FadeInIntro : MonoBehaviour
{
    public Image blackScreen; // Image component for the black screen
    public TMP_Text[] introTexts; // Array of texts for the intro screen
    public Dialogue dialogueSystem; // Reference to the Dialogue script
    public string introAudioName; // Name of the audio clip to play during the intro
    public string outroAudioName; // Name of the audio clip to fade out after the texts

    public float fadeDuration = 2.0f; // Duration of the fade effect
    public float textVisibleDuration = 1.5f; // Duration for which the texts remain fully visible
    public float textFadeDuration = 1.5f; // Duration for fade-out of the texts
    public float audioFadeDuration = 1.5f; // Duration for fading out the audio

    private void Start()
    {
        PlayerControlManager.Instance.DisablePlayerControls();
        StartCoroutine(FadeInSequence());
    }

    private IEnumerator FadeInSequence()
    {
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

        // Hide the black screen and texts after the fade-out
        blackScreen.gameObject.SetActive(false);

        // Activate the dialogue system after all texts fade out
        if (dialogueSystem != null)
        {
            dialogueSystem.gameObject.SetActive(true);
            dialogueSystem.StartDialogue(); // Ensure StartDialogue method exists and is accessible
        }
    }
}
