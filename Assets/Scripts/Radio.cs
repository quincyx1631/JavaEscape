using System.Collections;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform player;
    public Transform itemHolder;
    public GameObject battery;
    public float volumeStep = 0.1f;
    public float maxVolume = 1f;
    public float minVolume = 0f;
    public float maxDistance = 20f;
    private float currentVolume;
    private bool isRadioOn = false;

    // UI elements for volume up/down indicators
    public GameObject volumeUpIndicator;
    public GameObject volumeDownIndicator;
    private float indicatorDisplayTime = 0.5f;

    // Sound effects
    public string radioInteractSound; // Sound when the radio is turned on
    public string radioChangeSound;   // Sound when the volume is changed

    // Delay time for the AudioSource to play after radioInteractSound
    public float audioSourceDelay = 1f; // You can change this in the Inspector

    // Volume slider based on the Y-axis
    public Transform volumeSlider;  // The UI element (e.g., a slider) to move on the Y-axis
    public float minYPosition = 0f;  // Minimum Y position (for min volume)
    public float maxYPosition = 1f;  // Maximum Y position (for max volume)

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        currentVolume = minVolume;
        audioSource.volume = currentVolume;
        audioSource.loop = false;  // Make sure loop is unchecked in the Inspector
        audioSource.spatialBlend = 1f; // Set to 3D sound
    }

    private bool IsBatteryInHolder()
    {
        return battery != null && battery.transform.parent == itemHolder;
    }

    public void OpenRadio()
    {
        if (IsBatteryInHolder())
        {
            battery.transform.SetParent(null);
            battery.SetActive(false);

            isRadioOn = true;

            // Play the radio interaction sound first
            AudioManager.Instance.Play(radioInteractSound);

            // Start the coroutine to play the AudioSource sound after the delay
            StartCoroutine(PlayRadioWithDelay());
        }
        else
        {
            Debug.Log("Battery is not in the ItemHolder. Radio cannot play.");
        }
    }

    // Coroutine to play the radio sound with a 5-second delay in between each play
    private IEnumerator PlayRadioWithDelay()
    {
        // Wait for the initial delay before starting the audio
        yield return new WaitForSeconds(audioSourceDelay);

        // Keep playing the radio sound with a 5-second delay between each loop
        while (isRadioOn)
        {
            // Play the audio source sound
            audioSource.Play();

            // Wait until the audio finishes playing
            yield return new WaitForSeconds(audioSource.clip.length);

            // Wait for an additional 5 seconds after the audio finishes
            yield return new WaitForSeconds(5f);
        }
    }

    private void Update()
    {
        if (isRadioOn && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            float distanceFactor = Mathf.Clamp01(1 - (distance / maxDistance));
            audioSource.volume = currentVolume * distanceFactor;
        }

        // Update the volume slider based on the current volume
        UpdateVolumeSlider();
    }

    public void AdjustVolume()
    {
        if (!isRadioOn)
        {
            return;
        }

        float previousVolume = currentVolume;

        // Cycle volume up with each click
        currentVolume += volumeStep;
        if (currentVolume > maxVolume)
        {
            currentVolume = minVolume;
        }

        // Show appropriate volume indicator
        if (currentVolume > previousVolume)
        {
            StartCoroutine(ShowVolumeIndicator(volumeUpIndicator));
        }
        else if (currentVolume < previousVolume)
        {
            StartCoroutine(ShowVolumeIndicator(volumeDownIndicator));
        }

        // Play the sound effect when volume changes
        AudioManager.Instance.Play(radioChangeSound);
    }

    // Update the volume slider based on current volume
    private void UpdateVolumeSlider()
    {
        if (volumeSlider != null)
        {
            // Map the volume to the Y position range
            float normalizedVolume = Mathf.InverseLerp(minVolume, maxVolume, currentVolume);
            float newYPosition = Mathf.Lerp(minYPosition, maxYPosition, normalizedVolume);

            // Update the Y position of the slider
            volumeSlider.position = new Vector3(volumeSlider.position.x, newYPosition, volumeSlider.position.z);
        }
    }

    // Coroutine to briefly show the volume indicator
    private IEnumerator ShowVolumeIndicator(GameObject indicator)
    {
        indicator.SetActive(true);
        yield return new WaitForSeconds(indicatorDisplayTime);
        indicator.SetActive(false);
    }
}
