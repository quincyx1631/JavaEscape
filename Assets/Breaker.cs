using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ComputerPasswordPair
{
    public Computer computer; // Reference to the computer
    public string password; // Corresponding password for the keypad
}

public class Breaker : MonoBehaviour
{
    public List<ComputerPasswordPair> computerPasswordPairs; // List of computers and their passwords
    public KeypadNumbers keypad; // Reference to the keypad script
    public GameObject key; // Reference to the key GameObject
    public GameObject itemHolder; // Reference to the player's item holder GameObject
    public AlertUI alertUI;
    public Animator breakerAnimator; // Animator for the breaker animations
    private bool isUnlocked = false; // Flag to check if the breaker is unlocked
    private bool IsOpen = false; // Flag for the breaker being open or closed
    private bool breakerTurnedOn = false; // Flag to check if the breaker is already turned on

    private void Start()
    {
        // Initially set the breaker to be closed
        breakerAnimator.Play("BreakerClose");
        IsOpen = false;
        breakerTurnedOn = false; // Breaker hasn't been turned on yet
    }

    public void TurnOnBreaker()
    {
        // If the breaker is not unlocked, check for the key
        if (!isUnlocked)
        {
            // Check if the player has the key
            if (HasKeyInItemHolder())
            {
                UnlockAndOpenBreaker(); // Unlock and open the breaker in one step
                HideKey(); // Hide the key after unlocking the breaker
                return; // Exit after unlocking
            }
            else
            {
                alertUI.ShowAlert("You need the key to unlock the breaker.");
                Debug.Log("You need the key to unlock the breaker.");
                return; // Exit if the key is not present
            }
        }

        // If already unlocked, check if the breaker is already turned on
        if (!breakerTurnedOn && IsOpen)
        {
            // Turn on the breaker and close it only after activating the computer
            ActivateComputer();
            CloseBreaker(); // Play close animation after turning on the computer
            breakerTurnedOn = true; // Mark the breaker as turned on
        }
        else if (breakerTurnedOn)
        {
            alertUI.ShowAlert("The breaker is already turned on.");
            Debug.Log("The breaker is already turned on.");
        }
    }

    private bool HasKeyInItemHolder()
    {
        // Check if the key GameObject is a child of the item holder GameObject
        return key.transform.IsChildOf(itemHolder.transform);
    }

    private void UnlockAndOpenBreaker()
    {
        isUnlocked = true;
        CorrectUIController.Instance.ShowCorrectUI();
        Debug.Log("Breaker is unlocked and opened.");
        OpenBreaker(); // Play the open animation right after unlocking
    }

    private void HideKey()
    {
        // Hide the key GameObject after it has been used
        key.SetActive(false);
    }

    private void ActivateComputer()
    {
        // Randomly select a computer from the list
        int randomIndex = Random.Range(0, computerPasswordPairs.Count);
        ComputerPasswordPair selectedPair = computerPasswordPairs[randomIndex];

        // Open the selected computer
        selectedPair.computer.OpenComputer();

        // Set the keypad password to the corresponding password of the selected computer
        keypad.SetKeypadPassword(selectedPair.password);

        Debug.Log("Breaker turned on. Opened: " + selectedPair.computer.name + " with password: " + selectedPair.password);
    }

    private void OpenBreaker()
    {
        // Play the BreakerOpen animation and set the IsOpen flag
        breakerAnimator.Play("BreakerOpen");
        IsOpen = true;
        Debug.Log("Breaker is now open.");
    }

    private void CloseBreaker()
    {
        // Play the BreakerClose animation and set the IsOpen flag
        breakerAnimator.Play("BreakerClose");
        IsOpen = false;
        Debug.Log("Breaker is now closed.");
    }
}
