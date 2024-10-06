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
    private bool isUnlocked = false; // Flag to check if the breaker is unlocked

    public void TurnOnBreaker()
    {
        // If the breaker is not unlocked, check for the key
        if (!isUnlocked)
        {
            // Check if the player has the key
            if (HasKeyInItemHolder())
            {
                UnlockBreaker();
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

        // If already unlocked, now turn on the breaker and open the computer
        ActivateComputer();
    }

    private bool HasKeyInItemHolder()
    {
        // Check if the key GameObject is a child of the item holder GameObject
        return key.transform.IsChildOf(itemHolder.transform);
    }

    private void UnlockBreaker()
    {
        isUnlocked = true;
        CorrectUIController.Instance.ShowCorrectUI();
        Debug.Log("Breaker is unlocked. You can now turn it on.");
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
}
