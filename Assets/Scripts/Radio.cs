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

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        currentVolume = minVolume;
        audioSource.volume = currentVolume;
        audioSource.loop = true;
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
            StartCoroutine(PlayRadioAfterDelay());
        }
        else
        {
            Debug.Log("Battery is not in the ItemHolder. Radio cannot play.");
        }
    }

    // Coroutine to wait for the delay before playing the AudioSource
    private IEnumerator PlayRadioAfterDelay()
    {
        // Wait for the set delay time
        yield return new WaitForSeconds(audioSourceDelay);

        // Play the audio source sound after the delay
        audioSource.Play();
    }

    private void Update()
    {
        if (isRadioOn && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            float distanceFactor = Mathf.Clamp01(1 - (distance / maxDistance));
            audioSource.volume = currentVolume * distanceFactor;
        }
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

    // Coroutine to briefly show the volume indicator
    private IEnumerator ShowVolumeIndicator(GameObject indicator)
    {
        indicator.SetActive(true);
        yield return new WaitForSeconds(indicatorDisplayTime);
        indicator.SetActive(false);
    }
}
