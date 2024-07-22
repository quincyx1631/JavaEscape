using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
    public TMP_Text displayText; // Reference to the display text
    public string correctPassword = "ABCD"; // The correct password
    public Animator doorAnimator; // Reference to the door's Animator component
    public string nextSceneName = "NextScene"; // The name of the next scene

    private string currentInput = ""; // Stores the current input
    private Keypad3D keypad3D; // Reference to the Keypad3D script

    private void Start()
    {
        keypad3D = FindObjectOfType<Keypad3D>(); // Find the Keypad3D script in the scene
    }

    private void Update()
    {
        // Hide the keypad UI when the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            keypad3D.DisableKeypadUI();
        }
    }

    // Functions to handle button presses
    public void PressA() { AddLetter("A"); }
    public void PressB() { AddLetter("B"); }
    public void PressC() { AddLetter("C"); }
    public void PressD() { AddLetter("D"); }

    // Function to add a letter to the current input
    private void AddLetter(string letter)
    {
        if (currentInput.Length < correctPassword.Length)
        {
            currentInput += letter;
            UpdateDisplay();
        }
    }

    // Function to clear the input
    public void OnClear()
    {
        currentInput = "";
        UpdateDisplay();
    }

    // Function to check if the password is correct
    public void OnExecute()
    {
        if (currentInput == correctPassword)
        {
            displayText.text = "Correct!";
            // Trigger the door animation and handle scene transition
            SceneTransitionManager.Instance.LoadSceneAfterAnimation(nextSceneName, doorAnimator);
            keypad3D.DisableKeypadUI(); // Deactivate the keypad UI and hide the cursor
        }
        else
        {
            displayText.text = "Incorrect!";
        }
    }

    // Function to exit the keypad
    public void OnExit()
    {
        keypad3D.DisableKeypadUI(); // Deactivate the keypad UI and hide the cursor
    }

    // Function to update the display text
    private void UpdateDisplay()
    {
        displayText.text = currentInput;
    }
}
