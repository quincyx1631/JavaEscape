using UnityEngine;

public class Keypad3D : MonoBehaviour
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
        // Example: Disable player movement (adjust according to your player control setup)
        controller.playerCanMove = false;
        controller.cameraCanMove = false;


        // Example: Disable other scripts responsible for player movement
        // PlayerMovementScript.enabled = false;
    }

    private void EnablePlayerControls()
    {
        // Example: Enable player movement (adjust according to your player control setup)

        controller.playerCanMove = true;
        controller.cameraCanMove = true;
        // Example: Enable other scripts responsible for player movement
        // PlayerMovementScript.enabled = true;
    }
}
