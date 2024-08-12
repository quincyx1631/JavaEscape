using UnityEngine;

public class KeypadOBJ : MonoBehaviour
{
    public GameObject keypadUI; // Reference to the Keypad UI GameObject
    public FirstPersonController controller;


    // Function to enable the Keypad UI and the mouse cursor
    public void EnableKeypadUI()
    {
        if (keypadUI != null)
        {
            DisablePlayerControls();
            keypadUI.SetActive(true); // Enable the Keypad UI
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor
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
            EnablePlayerControls();
        }
    }
    private void DisablePlayerControls()
    {

        controller.enableCrouch = false;
        controller.enableJump = false;
        controller.playerCanMove = false;
        controller.cameraCanMove = false;
    }

    private void EnablePlayerControls()
    {
        controller.enableCrouch = true;
        controller.enableJump = true;
        controller.playerCanMove = true;
        controller.cameraCanMove = true;
    }
}
