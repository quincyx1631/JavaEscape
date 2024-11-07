using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LaptopPasswordGenerator : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] inputFields;    // Array of 10 input fields
    [SerializeField] private TMP_Text passwordDisplay;        // Text field to display password
    [SerializeField] private Button generateButton;           // Button to trigger password generation
    [SerializeField] private string[] correctAnswers;         // Array of correct answers for each input field
    [SerializeField] private TMP_Text[] answerTextFields;     // Array of 10 text fields to display correct answers
    public AlertUI alertUI;                                   // Reference to Alert UI for incorrect attempts
    public KeypadNumbers keypadNumbers;                       // Reference to KeypadNumbers to set the password
    public string typeSound;                                  // Name of the typing sound
    public string clickSound;                                 // Name of the button click sound
    public string generateSound;                              // Name of the generate sound

    private string finalPassword;                             // Holds the final 5-digit password

    private void Start()
    {
        passwordDisplay.text = "";                            // Ensure password display starts blank
        generateButton.onClick.AddListener(OnGenerateButtonClick);

        // Add typing sound to each input field
        foreach (var inputField in inputFields)
        {
            inputField.onValueChanged.AddListener(delegate { PlayTypeSound(); });
        }

        // Check if correctAnswers array matches inputFields array length
        if (correctAnswers.Length != inputFields.Length || answerTextFields.Length != inputFields.Length)
        {
            Debug.LogError("Error: The number of correct answers, input fields, and answer text fields must match.");
        }
    }

    private void OnGenerateButtonClick()
    {
        AudioManager.Instance.Play(clickSound);               // Play click sound
        GeneratePassword();
    }

    private void GeneratePassword()
    {
        if (CheckAnswers())
        {
            CorrectUIController.Instance.ShowCorrectUI();
            Debug.Log("All answers are correct. Generating password...");

            // Play generate sound at the start of password generation
            AudioManager.Instance.Play(generateSound);

            finalPassword = Generate5DigitPassword();
            StartCoroutine(AnimatePassword());

            // Use SetKeypadPassword to set the generated password in KeypadNumbers
            if (keypadNumbers != null)
            {
                keypadNumbers.SetKeypadPassword(finalPassword);
                Debug.Log("Password set in KeypadNumbers using SetKeypadPassword: " + finalPassword);
            }
            else
            {
                Debug.LogError("KeypadNumbers reference is missing.");
            }

            generateButton.interactable = false; // Disable the button after generating the password once
        }
        else
        {
            Debug.Log("One or more answers are incorrect. Please try again.");
        }
    }

    // Plays typing sound
    private void PlayTypeSound()
    {
        AudioManager.Instance.Play(typeSound);
    }

    // Checks if all input fields contain correct answers
    private bool CheckAnswers()
    {
        bool allCorrect = true;

        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i].text == correctAnswers[i])   // Case-sensitive comparison
            {
                Debug.Log($"Input field {i + 1} is correct. Answer: {correctAnswers[i]}");
                answerTextFields[i].text = correctAnswers[i]; // Display the correct answer in the text field
            }
            else
            {
                Debug.Log($"Input field {i + 1} is incorrect. Expected: '{correctAnswers[i]}', but got: '{inputFields[i].text}'");
                answerTextFields[i].text = "";               // Clear the text if the answer is incorrect
                allCorrect = false;
            }
        }

        if (!allCorrect)
        {
            alertUI.ShowAlert("Wrong Combinations");
        }

        return allCorrect;
    }

    // Generates a random 5-digit password
    private string Generate5DigitPassword()
    {
        string password = Random.Range(10000, 99999).ToString();
        Debug.Log($"Generated password: {password}");
        return password;
    }

    // Coroutine to animate the password reveal
    private IEnumerator AnimatePassword()
    {
        Debug.Log("Starting password animation...");
        float animationDuration = 3f;                       // Duration of the random number animation
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            passwordDisplay.text = Random.Range(10000, 99999).ToString();
            elapsedTime += 0.1f;
            yield return new WaitForSeconds(0.1f);          // Update every 0.1 seconds
            Debug.Log("Animating random numbers...");
        }

        passwordDisplay.text = finalPassword;               // Display the final password
        Debug.Log($"Final password displayed: {finalPassword}");
    }
}
