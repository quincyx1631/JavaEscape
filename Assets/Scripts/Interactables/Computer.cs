using UnityEngine;
using UnityEngine.UI; // For managing CanvasGroup (for alpha fading)
using System.Collections;

public class Computer : MonoBehaviour
{
    [Header("Computer Settings")]
    public string password; // Set this to the specific password for each computer

    [Header("UI and Audio Settings")]
    public CanvasGroup computerCanvas;   // Reference to the CanvasGroup of the entire UI, including background/material
    public AudioSource bootSound;        // Reference to the boot sound that plays when the computer turns on
    public float fadeDuration = 1.5f;    // Duration for the fade-in effect
    public float delayBeforeOpen = 1.0f; // Delay before starting to open the computer

    private bool isComputerOn = false;   // To track if the computer is on or off

    private void Start()
    {
        // Ensure the computer is initially turned off by setting its alpha to 0
        SetComputerUIAlpha(0);

        // Set the tag to "Untagged" when the computer is off
        gameObject.tag = "Untagged";
    }

    // This method will be called when the breaker is switched to turn on the computer
    public void OpenComputer()
    {
        if (!isComputerOn)
        {
            isComputerOn = true;  // Set computer state to "on"
            StartCoroutine(OpenComputerWithDelay());  // Start the delayed opening coroutine
        }
    }

    // Coroutine to handle delay before opening the computer
    private IEnumerator OpenComputerWithDelay()
    {
        // Wait for the specified delay before proceeding
        yield return new WaitForSeconds(delayBeforeOpen);

        // Start the fade-in coroutine
        StartCoroutine(FadeInComputerUI());

        // Play the boot sound
        PlayBootSound();

        // Change the tag to "Interactables" once the computer is on
        gameObject.tag = "Interactables";
    }

    // Coroutine to fade in the computer UI over a duration
    private IEnumerator FadeInComputerUI()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);  // Calculate the alpha
            SetComputerUIAlpha(alpha);  // Set the alpha value to the CanvasGroup
            yield return null;  // Wait for the next frame
        }

        // Ensure the UI is fully visible at the end
        SetComputerUIAlpha(1);
    }

    // Method to set the alpha of the computer's CanvasGroup (used for fading)
    private void SetComputerUIAlpha(float alpha)
    {
        if (computerCanvas != null)
        {
            computerCanvas.alpha = alpha;   // Set alpha transparency of the entire canvas (UI + material)
            computerCanvas.interactable = alpha > 0;  // Make the UI interactable only if visible
            computerCanvas.blocksRaycasts = alpha > 0;  // Enable raycasting only if visible
        }
    }

    // Method to play the boot-up sound effect
    private void PlayBootSound()
    {
        if (bootSound != null && !bootSound.isPlaying)
        {
            bootSound.Play();  // Play the boot-up sound
        }
    }
}
