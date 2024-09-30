using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Computer : MonoBehaviour
{
    [Header("Computer Settings")]
    public string password; // Password for this specific computer
    public bool isLoginComplete = false; // Track if login is complete for this computer

    [Header("UI and Audio Settings")]
    public CanvasGroup computerCanvas;   // Reference to the CanvasGroup of the entire UI
    public GameObject debugUI;           // Reference to this computer's specific Debug UI
    public GameObject worldSpaceDebugUI; // Reference to the World Space Debug UI
    public AudioSource bootSound;        // Boot sound for this computer
    public float fadeDuration = 1.5f;    // Duration of the fade-in effect
    public float delayBeforeOpen = 1.0f; // Delay before the computer turns on

    private bool isComputerOn = false;   // Track if the computer is on or off

    private void Start()
    {
        // Initially ensure the computer is off by setting alpha to 0
        SetComputerUIAlpha(0);

        // Disable the debug UI initially
        if (debugUI != null)
            debugUI.SetActive(false);
        
        // Disable the world space debug UI initially
        if (worldSpaceDebugUI != null)
            worldSpaceDebugUI.SetActive(false);
    }

    // Method to turn on the computer
    public void OpenComputer()
    {
        if (!isComputerOn)
        {
            isComputerOn = true;
            StartCoroutine(OpenComputerWithDelay());
        }
    }

    private IEnumerator OpenComputerWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeOpen);
        StartCoroutine(FadeInComputerUI());
        PlayBootSound();
        gameObject.tag = "Interactables"; // Mark the computer as interactable
    }

    private IEnumerator FadeInComputerUI()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetComputerUIAlpha(alpha);
            yield return null;
        }
        SetComputerUIAlpha(1);
    }

    private void SetComputerUIAlpha(float alpha)
    {
        if (computerCanvas != null)
        {
            computerCanvas.alpha = alpha;
            computerCanvas.interactable = alpha > 0;
            computerCanvas.blocksRaycasts = alpha > 0;
        }
    }

    private void PlayBootSound()
    {
        if (bootSound != null && !bootSound.isPlaying)
        {
            bootSound.Play();
        }
    }
}
