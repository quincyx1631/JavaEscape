using UnityEngine;

public class KeypadOBJ : MonoBehaviour
{
    public GameObject keypadUI; // Reference to the Keypad UI GameObject
    public GameObject escapeUI; // Reference to the Escape UI GameObject
    public Animator escapeUIAnimator; // Animator for the Escape UI
    public Door door; // Reference to the Door script
    public FirstPersonController controller;
    public CollectionUI collectionUI; // Reference to the CollectionUI script
    public AlertUI alertUI; // Reference to the AlertUI script
    public PlayerInteraction playerInteraction; // Reference to the PlayerInteraction script

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
        // Check if all items are collected before enabling the keypad UI
        if (collectionUI != null && collectionUI.AreAllItemsCollected())
        {
            if (keypadUI != null)
            {
                // Trigger the escape UI animation before showing the keypad UI
                if (escapeUI != null && escapeUIAnimator != null)
                {
                    escapeUI.SetActive(true);
                    escapeUIAnimator.SetBool("IsVisible", true); // Play the Escape UI animation
                }

                keypadUI.SetActive(true); // Enable the Keypad UI after Escape UI
                MouseManager.Instance.EnableMouse();
                PlayerControlManager.Instance.DisablePlayerControls();

                // Disable raycast for player interaction
                if (playerInteraction != null)
                {
                    playerInteraction.DisableRaycast();
                }
                else
                {
                    Debug.LogWarning("playerInteraction is not assigned.");
                }
            }
        }
        else
        {
            // Show alert if items are not collected
            if (alertUI != null)
            {
                alertUI.ShowAlert("You must collect all items before using the keypad.", "Alert");
            }
        }
    }

    // Function to disable the Keypad UI and the mouse cursor
    public void DisableKeypadUI()
    {
        if (keypadUI != null)
        {
            keypadUI.SetActive(false); // Disable the Keypad UI
            MouseManager.Instance.DisableMouse();
            PlayerControlManager.Instance.EnablePlayerControls();

            // Enable raycast for player interaction
            if (playerInteraction != null)
            {
                playerInteraction.EnableRaycast();
            }
            else
            {
                Debug.LogWarning("playerInteraction is not assigned.");
            }
        }

        // Hide the escape UI
        if (escapeUI != null && escapeUIAnimator != null)
        {
            escapeUIAnimator.SetBool("IsVisible", false); // Hide the Escape UI animation
        }
    }
}
