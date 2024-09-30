using UnityEngine;
using TMPro;

public class LoginUIManager : MonoBehaviour
{
    public static LoginUIManager Instance; // Singleton instance
    public TMP_InputField passwordInputField; // Reference to your password input field
    public AlertUI alertUI; // Reference to your AlertUI for messages

    private string currentPassword; // Store the current password

    private void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    public void SetPassword(string password)
    {
        currentPassword = password; // Store the password for this session
    }

    public void OnLoginButtonPressed()
    {
        if (passwordInputField.text == currentPassword)
        {
            // Password is correct
            Debug.Log("Login successful!");
            // Proceed with the next action (e.g., open debugging activities)
            // You can also hide the login UI here
        }
        else
        {
            // Password is incorrect
            alertUI.ShowAlert("Incorrect password, try again!");
        }
    }
}
