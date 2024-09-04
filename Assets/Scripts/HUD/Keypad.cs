using UnityEngine;
using TMPro;
using System.Collections;

public class Keypad : MonoBehaviour
{
    public TMP_Text displayText; // Reference to the display text
    [SerializeField] private string correctPassword = "ABCD"; // The correct password
    public Door door; // Reference to the Door script
    public KeypadOBJ keypadOBJ; // Reference to the KeypadOBJ script
    public GameObject clueUI; // Reference to the Clue UI GameObject
    public TMP_Text clueText; // Reference to the clue text in the Clue UI

    private string currentInput = ""; // Stores the current input
    private int incorrectAttempts = 0; // Counter for incorrect attempts

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
        clueUI.SetActive(false); // Ensure the Clue UI is initially hidden
    }

    private void Update()
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
            incorrectAttempts++; // Increment incorrect attempts

            StartCoroutine(ClearTextAfterDelay(1.5f)); // Clear the text after 1.5 seconds

            if (incorrectAttempts >= 3)
            {
                ShowClue();
            }
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

    private IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnClear(); // Clear the input and update the display after the delay
    }

    private void ShowClue()
    {
        clueUI.SetActive(true); // Show the Clue UI

        // Provide clues based on the number of incorrect attempts
        switch (incorrectAttempts)
        {
            case 3:
                clueText.text = "Clue: I think you are missing something.";
                break;
            case 4:
                clueText.text = "Clue: It seems another key might unlock the answer.";
                break;
            case 5:
                clueText.text = "Clue: The answer is closer than it appears.";
                break;
            case 6:
                clueText.text = "Clue: Check that drawer again, you might have overlooked something important.";
                break;
            default:
                clueText.text = "Keep searching, the truth lies hidden in plain sight.";
                break;
        }

        // Play the clue animation if applicable
        Animator clueAnimator = clueUI.GetComponent<Animator>();
        if (clueAnimator != null)
        {
            clueAnimator.SetTrigger("ShowClue"); // Trigger the clue animation
        }
    }

    public void SetPassword(string password)
    {
        correctPassword = password;
    }
}
