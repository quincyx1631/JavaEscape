using TMPro;
using UnityEngine;
using UnityEngine.Video; // For playing video on the TV

public class LaptopPasswordChecker : MonoBehaviour
{
    [Header("UI Settings")]
    public TMP_InputField passwordInputField; // The InputField for password entry

    [Header("Level Settings")]
    public LevelTwoGenerator levelTwoGenerator; // Reference to the LevelTwoGenerator script for the password

    [Header("Camera Settings")]
    public CameraSwitch cameraSwitch; // Reference to the CameraSwitch script for switching cameras

    [Header("TV Settings")]
    public GameObject tvScreen; // The GameObject with the TV video (should be disabled initially)

    [Header("Video Settings")]
    public VideoPlayer tvVideoPlayer; // The VideoPlayer component attached to the TV

    [Header("Alert Settings")]
    public AlertUI alertScript; // Reference to the AlertScript to show messages

    [Header("Laptop Settings")]
    public GameObject laptopCanvas; // The canvas that contains the laptop UI elements
    public GameObject laptopObject; // The laptop object itself (to change its tag)

    public void CheckPassword()
    {
        // Get the entered password from the input field
        string enteredPassword = passwordInputField.text;

        // Get the correct password from LevelTwoGenerator
        string correctPassword = levelTwoGenerator.GetLaptopPassword();

        if (enteredPassword.Equals(correctPassword))
        {
            Debug.Log("Password is correct!");

            // Switch back to the main camera using the existing CameraSwitch script
            cameraSwitch.SwitchToMainCamera();

            // Enable the TV screen and play the video
            if (tvScreen != null)
            {
                tvScreen.SetActive(true); // Enable the TV screen object (with the video)
            }

            if (tvVideoPlayer != null)
            {
                tvVideoPlayer.Play(); // Start playing the video on the TV
            }

            // Disable the laptop's canvas
            if (laptopCanvas != null)
            {
                laptopCanvas.SetActive(false); // Disable the laptop UI canvas
            }

            // Change the laptop's tag to "Untagged"
            if (laptopObject != null)
            {
                laptopObject.tag = "Untagged"; // Set the tag of the laptop to "Untagged"
            }

            // Clear the input field for future attempts if needed
            passwordInputField.text = "";
        }
        else
        {
            Debug.Log("Incorrect password, try again.");

            // Show an alert message for incorrect password using the AlertScript
            alertScript.ShowAlert("Incorrect password, please try again!");
        }
    }
}
