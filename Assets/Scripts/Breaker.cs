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

    // Rotation settings
    public Transform rotatingPart; // Reference to the child object you want to rotate (e.g., door or handle)
    public Vector3 closedRotation; // Rotation for the closed state (e.g., 0,0,0)
    public Vector3 openRotation;   // Rotation for the open state (e.g., 0,90,0)
    public float rotationSpeed = 2f; // Speed of rotation between states

    // Sound effects
    public string openSound = "OpenBreaker";   // Sound for opening the breaker
    public string closeSound = "CloseBreaker"; // Sound for closing the breaker

    private bool isUnlocked = false; // Flag to check if the breaker is unlocked
    private bool IsOpen = false; // Flag for the breaker being open or closed
    private bool breakerTurnedOn = false; // Flag to check if the breaker is already turned on
    private bool isRotating = false; // Flag to check if the breaker is in the process of rotating

    private void Start()
    {
        if (rotatingPart == null)
        {
            Debug.LogError("Rotating part is not assigned!");
        }

        rotatingPart.rotation = Quaternion.Euler(closedRotation);
        IsOpen = false;
        breakerTurnedOn = false;
    }

    private void Update()
    {
        if (isRotating)
        {
            RotatePart(IsOpen ? openRotation : closedRotation);
        }
    }

    public void TurnOnBreaker()
    {
        if (!isUnlocked)
        {
            if (HasKeyInItemHolder())
            {
                UnlockAndOpenBreaker();
                HideKey();
                return;
            }
            else
            {
                alertUI.ShowAlert("You need the key to unlock the breaker.");
                Debug.Log("You need the key to unlock the breaker.");
                return;
            }
        }

        if (!breakerTurnedOn && IsOpen)
        {
            ActivateComputer();
            CloseBreaker();
            breakerTurnedOn = true;
        }
        else if (breakerTurnedOn)
        {
            alertUI.ShowAlert("The breaker is already turned on.");
            Debug.Log("The breaker is already turned on.");
        }
    }

    private bool HasKeyInItemHolder()
    {
        return key.transform.IsChildOf(itemHolder.transform);
    }

    private void UnlockAndOpenBreaker()
    {
        isUnlocked = true;
        CorrectUIController.Instance.ShowCorrectUI();
        Debug.Log("Breaker is unlocked and opening.");
        OpenBreaker();
    }

    private void HideKey()
    {
        key.SetActive(false);
    }

    private void ActivateComputer()
    {
        int randomIndex = Random.Range(0, computerPasswordPairs.Count);
        ComputerPasswordPair selectedPair = computerPasswordPairs[randomIndex];

        selectedPair.computer.OpenComputer();
        keypad.SetKeypadPassword(selectedPair.password);

        Debug.Log("Breaker turned on. Opened: " + selectedPair.computer.name + " with password: " + selectedPair.password);
    }

    private void OpenBreaker()
    {
        IsOpen = true;
        StartRotatingToTarget(openRotation);
        AudioManager.Instance.Play(openSound);  // Play open sound effect
        Debug.Log("Breaker is now open.");
    }

    private void CloseBreaker()
    {
        IsOpen = false;
        StartRotatingToTarget(closedRotation);
        AudioManager.Instance.Play(closeSound); // Play close sound effect
        Debug.Log("Breaker is now closed.");
    }

    private void StartRotatingToTarget(Vector3 targetRotation)
    {
        isRotating = true;
    }

    private void RotatePart(Vector3 targetRotation)
    {
        Quaternion target = Quaternion.Euler(targetRotation);
        rotatingPart.rotation = Quaternion.Lerp(rotatingPart.rotation, target, Time.deltaTime * rotationSpeed);

        if (Quaternion.Angle(rotatingPart.rotation, target) < 0.1f)
        {
            rotatingPart.rotation = target;
            isRotating = false;
        }
    }
}
