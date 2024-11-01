using UnityEngine;
using System.Collections;

public class VideoAudioFader : MonoBehaviour
{
    public AudioSource tvAudioSource;   // The AudioSource attached to the TV
    public float fadeDuration = 10f;    // Time in seconds to fade the audio
    public float delayBeforeFading = 5f; // Time in seconds to wait before starting the fade

    public void StartFading()
    {
        // Start the fading process, but only after the delay
        StartCoroutine(FadeOutAudioAfterDelay());
    }

    // Coroutine to handle the delay and then fade out the audio over time
    private IEnumerator FadeOutAudioAfterDelay()
    {
        // Ensure the audio starts at full volume
        if (tvAudioSource != null)
        {
            tvAudioSource.volume = 1f;  // Ensure audio is full volume at the start
        }

        // Wait for the delay before fading
        yield return new WaitForSeconds(delayBeforeFading);

        // Start fading the audio over the duration
        float startVolume = tvAudioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Gradually reduce the volume over time
            tvAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the volume is set to zero at the end
        tvAudioSource.volume = 0f;
    }
}
