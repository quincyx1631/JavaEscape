using UnityEngine;

public class KeypadOBJ : MonoBehaviour
{
    public GameObject keypadUI; // Reference to the Keypad UI GameObject
    public Door door; // Reference to the Door script
    public FirstPersonController controller;

    // Function to handle password input
    public void CheckPassword(string enteredPassword)
    {
        if (IsPasswordCorrect(enteredPassword))
        {
            door.UnlockDoor(); // Unlock the door
            DisableKeypadUI(); // Hide the keypad UI
            Debug.Log("Password is correct. Door unlocked.");
        }
        else
        {
            Debug.Log("Password is incorrect.");
        }
    }

    private bool IsPasswordCorrect(string enteredPassword)
    {
        // Replace with your actual password validation logic
        return enteredPassword == "YourPassword";
    }

    // Function to enable the Keypad UI and the mouse cursor
    public void EnableKeypadUI()
    {
        if (keypadUI != null)
        {
            keypadUI.SetActive(true); // Enable the Keypad UI
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor
            PlayerControlManager.Instance.DisablePlayerControls();
        }
    }

    // Function to disable the Keypad UI and the mouse cursor
    public void DisableKeypadUI()
    {
        if (keypadUI != null)
        {
            keypadUI.SetActive(false); // Disable the Keypad UI
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Hide the cursor
            PlayerControlManager.Instance.EnablePlayerControls();

        }
    }
}
