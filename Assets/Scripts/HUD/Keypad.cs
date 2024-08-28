using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
    public TMP_Text displayText; // Reference to the display text
    [SerializeField] private string correctPassword = "ABCD"; // The correct password
    public Door door; // Reference to the Door script
    public KeypadOBJ keypadOBJ; // Reference to the KeypadOBJ script
    private string currentInput = ""; // Stores the current input

    // Names of the sounds in your audio manager
    public string keyPressSound = "KeypadPress";
    public string correctSound = "Correct";
    public string incorrectSound = "Incorrect";

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
            PlaySound(keyPressSound); // Play the key press sound
        }
    }

    public void OnClear()
    {
        currentInput = "";
        UpdateDisplay();
        PlaySound(keyPressSound); // Play the key press sound on clear as well
    }

    public void OnExecute()
    {
        if (currentInput == correctPassword)
        {
            displayText.text = "Correct!";
            Debug.Log("Correct password entered.");
            PlaySound(correctSound); // Play the correct sound

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
            PlaySound(incorrectSound); // Play the incorrect sound
        }
    }

    private void UpdateDisplay()
    {
        displayText.text = currentInput;
    }

    private void PlaySound(string soundName)
    {
        // Assuming you have a method in your audio manager like PlaySound
        AudioManager.Instance.Play(soundName);
    }
}
