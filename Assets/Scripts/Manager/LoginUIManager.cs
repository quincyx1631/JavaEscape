using UnityEngine;
using TMPro;

public class LoginUIManager : MonoBehaviour
{
    public static LoginUIManager Instance; // Singleton instance for easy access

    public TMP_InputField passwordInputField; // Reference to the password input field
    public AlertUI alertUI;                  // Reference to the alert UI for showing messages
    public GameObject loginUI;               // Reference to the Login UI

    private string currentPassword;          // Current password to check
    private Computer currentComputer;        // Reference to the currently interacting computer

    public string typingSoundName; // Sound for typing
    public string loginSoundName;  // Sound for login button press
    public string debugSoundName;  // Sound for debug button press

    private void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    private void Start()
    {
        if (passwordInputField != null)
        {
            passwordInputField.contentType = TMP_InputField.ContentType.Password; // Set input field to password mode
            passwordInputField.onValueChanged.AddListener(OnTyping); // Add listener for typing
        }
    }


    public void SetPassword(string password)
    {
        currentPassword = password; // Store the password for the current session
    }

    public void SetCurrentComputer(Computer computer)
    {
        currentComputer = computer; // Set the current computer reference
        SetPassword(computer.password); // Set the current password from the computer
    }

    public void ShowLoginUI()
    {
        loginUI.SetActive(true); // Show the login UI
        HideDebugUI();           // Ensure the Debug UI is hidden
    }

    public void ShowDebugUI()
    {
        HideLoginUI();

        // Hide the world-space login UI (computerCanvas)
        if (currentComputer.computerCanvas != null)
        {
            currentComputer.computerCanvas.alpha = 0;
            currentComputer.computerCanvas.interactable = false;
            currentComputer.computerCanvas.blocksRaycasts = false;
        }

        // Show the regular Debug UI
        if (currentComputer != null && currentComputer.debugUI != null)
        {
            currentComputer.debugUI.SetActive(true); // Show the specific Debug UI for the current computer
        }

        // Show the World Space Debug UI
        if (currentComputer != null && currentComputer.worldSpaceDebugUI != null)
        {
            currentComputer.worldSpaceDebugUI.SetActive(true); // Show the World Space Debug UI for this computer
        }
    }

    public void HideLoginUI()
    {
        loginUI.SetActive(false); // Hide the login UI
        passwordInputField.text = ""; // Clear the input field
    }


    public void HideDebugUI()
    {
        if (currentComputer != null && currentComputer.debugUI != null)
        {
            currentComputer.debugUI.SetActive(false); // Hide the specific Debug UI
        }

        if (currentComputer != null && currentComputer.worldSpaceDebugUI != null)
        {
            currentComputer.worldSpaceDebugUI.SetActive(false); // Hide the World Space Debug UI
        }
    }

    private void OnTyping(string currentText)
    {
        AudioManager.Instance.Play(typingSoundName); // Play typing sound
    }

    public void OnLoginButtonPressed()
    {
        AudioManager.Instance.Play(loginSoundName); // Play login button sound

        if (passwordInputField.text == currentPassword)
        {
            // Password is correct
            Debug.Log("Login successful!");
            currentComputer.isLoginComplete = true; // Mark login as complete for the current computer
            ShowDebugUI(); // Show both Debug UIs (regular and world space)
        }
        else
        {
            // Password is incorrect
            alertUI.ShowAlert("Incorrect password, try again!");
        }
    }

    public void OnDebugButtonPressed()
    {
        AudioManager.Instance.Play(debugSoundName); // Play debug button sound

        if (currentComputer != null)
        {
            string userCode = currentComputer.GetDebugInput(); // Get the user input from the specific debug input field
            string feedback = currentComputer.ValidateDebugCode(userCode); // Validate the debug code

            // Show alert for incorrect feedback
            if (feedback != "Debug code accepted.")
            {
                alertUI.ShowAlert(feedback); // Show alert for feedback if debug code is incorrect
            }
            else
            {
                // Logic for handling correct debug code
                currentComputer.ToggleDebugUI(false); // Hide both Debug UIs
                currentComputer.ShowClueUI(); // Show the clue UI

                // Destroy computerCanvas and worldSpaceDebugUI
                if (currentComputer.computerCanvas != null)
                {
                    Destroy(currentComputer.computerCanvas.gameObject); // Destroy the computerCanvas GameObject
                }

                if (currentComputer.worldSpaceDebugUI != null)
                {
                    Destroy(currentComputer.worldSpaceDebugUI); // Destroy the World Space Debug UI GameObject
                }

                // Open the next computer directly from LoginUIManager
                if (currentComputer.nextComputer != null)
                {
                    Debug.Log("Opening next computer: " + currentComputer.nextComputer.name);
                    currentComputer.nextComputer.OpenComputer(); // Open the next computer
                }
                else
                {
                    Debug.LogWarning("Next computer is not assigned for: " + currentComputer.name);
                }
            }
        }
    }
}
