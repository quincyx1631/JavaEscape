using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertUI : MonoBehaviour
{
    public TextMeshProUGUI alertText; // Reference to the TextMeshProUGUI component
    public Image alertBackground; // Reference to the Image component (background)
    public float fadeDuration = 1f; // Duration of the fade out
    public float displayDuration = 2f; // Duration the alert is displayed before fading
    public string defaultAlertSoundName; // The default alert sound name

    public float shakeDuration = 0.3f; // Duration of the camera shake
    public float shakeMagnitude = 0.1f; // Magnitude of the camera shake

    private Camera mainCamera; // Reference to the main camera
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensure the alert starts invisible and disabled
        SetAlpha(0f);
        gameObject.SetActive(false);

        // Get the main camera
        mainCamera = Camera.main;
    }

    // Call this method to show an alert with a specific message and an optional sound name
    public void ShowAlert(string message, string alertSoundName = null)
    {
        gameObject.SetActive(true); // Enable the UI when showing the alert
        alertText.text = message;
        StopAllCoroutines(); // Stop any existing fade out

        // Use the provided sound name, or fall back to the default
        string soundToPlay = string.IsNullOrEmpty(alertSoundName) ? defaultAlertSoundName : alertSoundName;
        PlayAlertSound(soundToPlay);

        StartCoroutine(FadeInAndOut());

        // Trigger the camera shake effect
        if (mainCamera != null)
        {
            StartCoroutine(CameraShake());
        }
        else
        {
            Debug.LogWarning("Main camera not found.");
        }
    }

    // Call this method to play the alert sound using the audio manager
    private void PlayAlertSound(string soundName)
    {
        if (!string.IsNullOrEmpty(soundName))
        {
            AudioManager.Instance.Play(soundName);
        }
        else
        {
            Debug.LogWarning("Alert sound name is not set.");
        }
    }

    // Coroutine to handle the camera shake effect
    private IEnumerator CameraShake()
    {
        Vector3 originalPos = mainCamera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }

    // Coroutine to handle the fade in and out effect
    private IEnumerator FadeInAndOut()
    {
        // Fade in
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0f, 1f, timer / fadeDuration));
            yield return null;
        }

        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1f, 0f, timer / fadeDuration));
            yield return null;
        }

        // Ensure the alert is fully hidden at the end
        SetAlpha(0f);
        gameObject.SetActive(false); // Disable the UI after it finishes fading out
    }

    // Sets the alpha of both the text and the background image
    private void SetAlpha(float alpha)
    {
        if (alertText != null)
        {
            alertText.alpha = alpha;
        }
        if (alertBackground != null)
        {
            Color bgColor = alertBackground.color;
            bgColor.a = alpha;
            alertBackground.color = bgColor;
        }
    }
}
