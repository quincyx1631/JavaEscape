using UnityEngine;
using TMPro;
using System.Collections;

public class KeypadNumbers : MonoBehaviour
{
    public TMP_Text displayText; // Reference to the display text
    [SerializeField] private string correctPassword = "1234567890"; // The correct numeric password
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

    public void Press1() { AddNumber("1"); }
    public void Press2() { AddNumber("2"); }
    public void Press3() { AddNumber("3"); }
    public void Press4() { AddNumber("4"); }
    public void Press5() { AddNumber("5"); }
    public void Press6() { AddNumber("6"); }
    public void Press7() { AddNumber("7"); }
    public void Press8() { AddNumber("8"); }
    public void Press9() { AddNumber("9"); }
    public void Press0() { AddNumber("0"); }

    private void AddNumber(string number)
    {
        if (currentInput.Length < correctPassword.Length)
        {
            currentInput += number;
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
                clueText.text = "Clue: Perhaps the answer lies in a sequence.";
                break;
            case 4:
                clueText.text = "Clue: You're on the right track, but still far.";
                break;
            case 5:
                clueText.text = "Clue: Take a closer look at the numbers around you.";
                break;
            case 6:
                clueText.text = "Clue: Pay attention to repeating patterns.";
                break;
            default:
                clueText.text = "Keep trying, the answer is within reach!";
                break;
        }

        // Play the clue animation if applicable
        Animator clueAnimator = clueUI.GetComponent<Animator>();
        if (clueAnimator != null)
        {
            clueAnimator.SetTrigger("ShowClue"); // Trigger the clue animation
        }
    }

    // Method to set the keypad password externally
    public void SetKeypadPassword(string newPassword)
    {
        correctPassword = newPassword;
        Debug.Log("Keypad password updated: " + newPassword);
    }
}
