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

    private void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    public void SetPassword(string password)
    {
        currentPassword = password; // Store the password for the current session
    }

    public void SetCurrentComputer(Computer computer)
    {
        currentComputer = computer; // Set the current computer reference
    }

    public void ShowLoginUI()
    {
        loginUI.SetActive(true); // Show the login UI
        HideDebugUI();           // Ensure the Debug UI is hidden
    }

    public void ShowDebugUI()
    {
        HideLoginUI();

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

    public void OnLoginButtonPressed()
    {
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
}
