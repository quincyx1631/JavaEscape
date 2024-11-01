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

    [Header("Combination Settings")]
    public Color[] availableColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta };
    private List<int> currentCombination = new List<int>();
    public int[] correctCombination; // Set this in the Inspector

    private string generatedPassword = "";
    private bool isAnimating = false;
    private bool hasGeneratedPassword = false; // Flag to prevent multiple generations
    private List<Color> originalColors = new List<Color>();

    public KeypadNumbers keypadNumbers; // Reference to KeypadNumbers script

    void Start()
    {
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

        // Cycle through colors for the specified object
        currentCombination[objectIndex] = (currentCombination[objectIndex] + 1) % availableColors.Length;
        SetColor(objectIndex, currentCombination[objectIndex]);
    }

    public void CheckCombination()
    {
        if (isAnimating || hasGeneratedPassword) return; // Skip if animation is running or password already generated

        // Verify if the current combination matches the correct one
        bool isCorrect = true;
        for (int i = 0; i < correctCombination.Length; i++)
        {
            if (currentCombination[i] != correctCombination[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
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
        displayText.text = "W R O N G";
        displayText.color = Color.red;
    }
}
