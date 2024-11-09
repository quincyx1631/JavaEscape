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
            audioSource.Play();
        }
        else
        {
            Debug.Log("Battery is not in the ItemHolder. Radio cannot play.");
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
    }

    // Coroutine to briefly show the volume indicator
    private IEnumerator ShowVolumeIndicator(GameObject indicator)
    {
        indicator.SetActive(true);
        yield return new WaitForSeconds(indicatorDisplayTime);
        indicator.SetActive(false);
    }
}
