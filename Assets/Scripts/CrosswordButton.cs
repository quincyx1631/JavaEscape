using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrosswordButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI buttonText;
    public string correctLetter;
    private bool isLocked = false;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);

        // Start with an empty button text
        buttonText.text = "";
    }

    private void OnButtonClick()
    {
        if (isLocked) return; // Skip if button is locked

        // If button text is blank, start with 'A'
        if (string.IsNullOrEmpty(buttonText.text))
        {
            buttonText.text = "A";
        }
        else
        {
            // Cycle to the next letter in the alphabet
            char nextLetter = buttonText.text[0] == 'Z' ? 'A' : (char)(buttonText.text[0] + 1);
            buttonText.text = nextLetter.ToString();
        }
    }

    // Check if the current letter is correct
    public bool IsCorrect()
    {
        return buttonText.text == correctLetter;
    }

    // Lock the button when the word is completed
    public void LockButton()
    {
        isLocked = true;
        button.interactable = false; // Disable the button
    }
}
