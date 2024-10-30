using TMPro;
using UnityEngine;
using System.Collections;

public class Computer : MonoBehaviour
{
    [Header("Computer Settings")]
    public string password; // Password for this specific computer
    [TextArea] public string debugCode; // Debug code for this specific computer
    public bool isLoginComplete = false;
    public bool isDebugAnswered;
    public Computer nextComputer; // Track the next computer to open if login is complete

    [Header("UI and Audio Settings")]
    public CanvasGroup computerCanvas; // Reference to the CanvasGroup of the entire UI
    public GameObject debugUI; // Reference to this computer's specific Debug UI
    public GameObject worldSpaceDebugUI; // Reference to the World Space Debug UI
    public TMP_InputField debugInputField; // Specific debug input field for this computer
    public float fadeDuration = 1.5f; // Duration of the fade-in effect
    public float delayBeforeOpen = 1.0f; // Delay before the computer turns on
    public GameObject clueUI; // Reference to the clue UI GameObject

    private bool isComputerOn = false; // Track if the computer is on or off
    public string typingSoundName;
    public string bootSoundName;
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

        // Disable the clue UI initially
        if (clueUI != null)
            clueUI.SetActive(false);

        // Add typing sound listener to the input field
        if (debugInputField != null)
        {
            debugInputField.onValueChanged.AddListener(OnTyping);
        }
    }

    // Method to turn on the computer
    public void OpenComputer()
    {
        if (!isComputerOn)
        {
            isComputerOn = true;
            gameObject.tag = "Interactables"; // Mark the computer as interactable
            Debug.Log("Opening computer: " + gameObject.name);
            StartCoroutine(OpenComputerWithDelay());
        }
        else
        {
            Debug.Log("Computer is already on.");
        }
    }

    private IEnumerator OpenComputerWithDelay()
    {
        Debug.Log("Waiting before opening computer...");
        yield return new WaitForSeconds(delayBeforeOpen);
        StartCoroutine(FadeInComputerUI());
        AudioManager.Instance.Play(bootSoundName); // Use AudioManager for boot sound
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

    private void OnTyping(string currentText)
    {
        AudioManager.Instance.Play(typingSoundName); // Play typing sound on each character input
    }

    public string ValidateDebugCode(string inputCode)
    {
        if (inputCode == debugCode)
        {
            gameObject.tag = "Untagged"; // Set the computer's tag to "Untagged" after debug code is accepted
            Debug.Log("Debug code accepted for computer: " + gameObject.name);

            // Mark the computer as collected in the CollectionManager
            CollectionManager.Instance.MarkAsCollected(this.GetComponent<Interactables>());

            return "Debug code accepted.";
        }
        else
        {
            Debug.Log("Invalid debug code entered for computer: " + gameObject.name);
            return "Invalid debug code.";
        }
    }

    public void ToggleDebugUI(bool show)
    {
        if (debugUI != null)
            debugUI.SetActive(show);
        if (worldSpaceDebugUI != null)
            worldSpaceDebugUI.SetActive(show);
    }

    public void ShowClueUI()
    {
        if (clueUI != null)
        {
            clueUI.SetActive(true); // Activate the clue UI
            Debug.Log("Clue UI activated for computer: " + gameObject.name);
        }
    }

    public string GetDebugInput()
    {
        return debugInputField != null ? debugInputField.text : string.Empty; // Return the debug input
    }

    public void AnswerDebugQuestion(bool isCorrect)
    {
        if (isCorrect)
        {
            isDebugAnswered = true; // Set to true if the debug question was answered correctly
            Debug.Log("Debug question answered correctly for computer: " + gameObject.name);

            // Show the clue UI
            ShowClueUI();

            // Automatically open the next computer if it's set
            if (nextComputer != null)
            {
                Debug.Log("Opening next computer: " + nextComputer.name);
                nextComputer.OpenComputer(); // Open the next computer
            }
            else
            {
                Debug.LogWarning("Next computer is not assigned for: " + gameObject.name);
            }
        }
        else
        {
            Debug.Log("Debug question answered incorrectly for computer: " + gameObject.name);
        }
    }
}
