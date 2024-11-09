using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorCombinationDevice : MonoBehaviour
{
    [Header("Device Setup")]
    public GameObject[] colorObjects;  // Five 3D objects for color-changing
    public GameObject checkObject;      // Object to check the combination
    public TextMeshProUGUI displayText; // TMP for displaying results and password
    public TextMeshProUGUI clueText;    // TMP for displaying clues
    public string clickSoundName;       // Sound for color button clicks
    public string checkSoundName;       // Sound for check button click
    public string wrongSoundName;       // Sound for wrong answer
    public string generateSoundName;    // Sound for password generation

    [Header("Combination Settings")]
    public Color[] availableColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta };

    private List<int> currentCombination = new List<int>();

    // Struct to hold a combination and its associated clue
    [System.Serializable]
    public struct CombinationData
    {
        public int[] combination; // Correct combination
        [TextArea] public string clue;       // Associated clue
    }

    public CombinationData[] combinations; // Array to hold multiple combinations

    private string generatedPassword = "";
    private bool isAnimating = false;
    private bool hasGeneratedPassword = false; // Flag to prevent multiple generations
    private List<Color> originalColors = new List<Color>();

    private CombinationData selectedCombination; // Randomly selected combination and clue

    public KeypadNumbers keypadNumbers; // Reference to KeypadNumbers script

    void Start()
    {
        // Randomly select one combination and display its clue
        if (combinations.Length > 0)
        {
            int randomIndex = Random.Range(0, combinations.Length);
            selectedCombination = combinations[randomIndex];
            clueText.text = selectedCombination.clue;
        }

        // Store original colors and initialize combination
        foreach (var obj in colorObjects)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                originalColors.Add(renderer.material.color);
            }
            currentCombination.Add(0); // Start with the first color
        }

        ResetColors();
    }

    public void ChangeColor(int objectIndex)
    {
        if (isAnimating || hasGeneratedPassword) return; // Skip if animation is running or password already generated

        // Play click sound
        AudioManager.Instance.Play(clickSoundName);

        // Cycle through colors for the specified object
        currentCombination[objectIndex] = (currentCombination[objectIndex] + 1) % availableColors.Length;
        SetColor(objectIndex, currentCombination[objectIndex]);
    }

    public void CheckCombination()
    {
        if (isAnimating || hasGeneratedPassword) return; // Skip if animation is running or password already generated

        // Play check button sound
        AudioManager.Instance.Play(checkSoundName);

        // Check if the current combination matches the selected correct combination
        if (CheckIfCorrect(currentCombination, selectedCombination.combination))
        {
            // Start the password generation animation
            StartCoroutine(GeneratePasswordAnimation());

            // Set all color objects' tags to "Untagged"
            foreach (var obj in colorObjects)
            {
                obj.tag = "Untagged";
            }

            // Set checkObject's tag to "Untagged"
            checkObject.tag = "Untagged";

            // Mark as password generated
            hasGeneratedPassword = true;
        }
        else
        {
            DisplayWrongMessage();
        }
    }

    private bool CheckIfCorrect(List<int> current, int[] correct)
    {
        if (current.Count != correct.Length) return false;
        for (int i = 0; i < correct.Length; i++)
        {
            if (current[i] != correct[i])
            {
                return false;
            }
        }
        return true;
    }

    void SetColor(int index, int colorIndex)
    {
        // Apply selected color to the object's material
        var renderer = colorObjects[index].GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = availableColors[colorIndex];
        }
    }

    void ResetColors()
    {
        // Set each object to its original color
        for (int i = 0; i < colorObjects.Length; i++)
        {
            var renderer = colorObjects[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColors[i];
            }
        }
    }

    IEnumerator GeneratePasswordAnimation()
    {
        isAnimating = true;

        // Play password generation sound
        AudioManager.Instance.Play(generateSoundName);

        displayText.color = Color.black;

        // Generate a random 5-digit password
        generatedPassword = Random.Range(10000, 99999).ToString();

        // Animation: Display random numbers for 2 seconds
        float animationTime = 2f;
        float elapsedTime = 0f;
        while (elapsedTime < animationTime)
        {
            displayText.text = Random.Range(10000, 99999).ToString();
            elapsedTime += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }

        // Show the generated password in black
        displayText.text = generatedPassword;
        isAnimating = false;

        // Set the password in KeypadNumbers
        if (keypadNumbers != null)
        {
            keypadNumbers.SetKeypadPassword(generatedPassword);
        }
    }

    void DisplayWrongMessage()
    {
        // Play wrong answer sound
        AudioManager.Instance.Play(wrongSoundName);

        displayText.text = "W R O N G";
        displayText.color = Color.red;
    }
}
