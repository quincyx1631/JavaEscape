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
    public string radioInteractSound;
    public string radioChangeSound;

    // Delay time for the AudioSource to play after radioInteractSound
    public float audioSourceDelay = 1f;

    // Volume slider based on the Y-axis
    public Transform volumeSlider;
    public float minYPosition = 0f;
    public float maxYPosition = 1f;

    public AlertUI alertUI;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Initially disable the AudioSource
        audioSource.enabled = false;

        currentVolume = minVolume;
        audioSource.volume = currentVolume;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f;
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

            // Play the radio interaction sound
            AudioManager.Instance.Play(radioInteractSound);
            CollectionManager.Instance.MarkAsCollected(this.GetComponent<Interactables>());

            // Enable the audio source and start the coroutine to play it after a delay
            audioSource.enabled = true;
            StartCoroutine(PlayRadioWithDelay());
            gameObject.tag = "Untagged";
        }
        else
        {
            alertUI.ShowAlert("The radio needs a battery");
            Debug.Log("Battery is not in the ItemHolder. Radio cannot play.");
        }
    }

    private IEnumerator PlayRadioWithDelay()
    {
        // Wait for the initial delay before starting the audio
        yield return new WaitForSeconds(audioSourceDelay);

        // Keep playing the radio sound with a 5-second delay between each loop
        while (isRadioOn)
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
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

        UpdateVolumeSlider();
    }

    public void AdjustVolume()
    {
        if (!isRadioOn)
        {
            return;
        }

        float previousVolume = currentVolume;

        currentVolume += volumeStep;
        if (currentVolume > maxVolume)
        {
            currentVolume = minVolume;
        }

        if (currentVolume > previousVolume)
        {
            StartCoroutine(ShowVolumeIndicator(volumeUpIndicator));
        }
        else if (currentVolume < previousVolume)
        {
            StartCoroutine(ShowVolumeIndicator(volumeDownIndicator));
        }

        AudioManager.Instance.Play(radioChangeSound);
    }

    private void UpdateVolumeSlider()
    {
        if (volumeSlider != null)
        {
            float normalizedVolume = Mathf.InverseLerp(minVolume, maxVolume, currentVolume);
            float newYPosition = Mathf.Lerp(minYPosition, maxYPosition, normalizedVolume);
            volumeSlider.position = new Vector3(volumeSlider.position.x, newYPosition, volumeSlider.position.z);
        }
    }

    private IEnumerator ShowVolumeIndicator(GameObject indicator)
    {
        indicator.SetActive(true);
        yield return new WaitForSeconds(indicatorDisplayTime);
        indicator.SetActive(false);
    }

    // Optional function to turn off the radio and disable the AudioSource
    public void CloseRadio()
    {
        isRadioOn = false;
        audioSource.Stop();
        audioSource.enabled = false;
    }
}
