using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
    public TMP_Text displayText; // Reference to the display text
    [SerializeField] public string correctPassword = "ABCD"; // The correct password
    public Door door; // Reference to the Door script
    public KeypadOBJ keypadOBJ; // Reference to the KeypadOBJ script
    private string currentInput = ""; // Stores the current input

    private void Start()
    {
        if (keypadOBJ != null)
        {
            keypadOBJ.EnableKeypadUI(); // Show the keypad UI at the start or when needed
        }
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            keypadOBJ.DisableKeypadUI();
        }
    }

    public void PressA() { AddLetter("A"); }
    public void PressB() { AddLetter("B"); }
    public void PressC() { AddLetter("C"); }
    public void PressD() { AddLetter("D"); }

    private void AddLetter(string letter)
    {
        if (currentInput.Length < correctPassword.Length)
        {
            currentInput += letter;
            UpdateDisplay();
        }
    }

    public void OnClear()
    {
        currentInput = "";
        UpdateDisplay();
    }

    public void OnExecute()
    {
        if (currentInput == correctPassword)
        {
            displayText.text = "Correct!";
            Debug.Log("Correct password entered.");

            if (door != null)
            {
                door.UnlockDoor(); // Unlock the door if the password is correct
            }
            else
            {
                Debug.LogError("Door reference is missing.");
            }

            if (keypadOBJ != null)
            {
                keypadOBJ.DisableKeypadUI(); // Disable the keypad UI
            }
        }
        else
        {
            displayText.text = "Incorrect!";
        }
    }

    private void UpdateDisplay()
    {
        displayText.text = currentInput;
    }
}
