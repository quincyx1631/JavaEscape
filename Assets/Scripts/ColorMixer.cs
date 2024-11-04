using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorMixer : MonoBehaviour
{
    [Header("Primary Colors")]
    public Color redColor;
    public Color blueColor;
    public Color yellowColor;
    public Color greenColor;
    public Color purpleColor;

    [Header("Resulting Colors")]
    public Color redYellow;           // Red + Yellow
    public Color blueYellow;          // Blue + Yellow
    public Color redBlue;             // Red + Blue
    public Color blueGreen;           // Blue + Green
    public Color bluePurple;          // Blue + Purple
    public Color redGreen;            // Red + Green
    public Color redPurple;           // Red + Purple
    public Color yellowGreen;         // Yellow + Green
    public Color greenPurple;         // Green + Purple
    public Color yellowPurple;        // Yellow + Purple

    [Header("Color Setup")]
    public Image[] colorPlaceholders; // Assign the placeholder images in the Inspector

    [Header("Color Buttons")]
    public Button redButton;
    public Button blueButton;
    public Button yellowButton;
    public Button greenButton;
    public Button purpleButton;
    public Button clearButton;

    public string clickSoundName;

    private Color[] placeholderColors; // Array to hold the colors of the placeholders

    void Start()
    {
        if (colorPlaceholders == null || colorPlaceholders.Length < 3)
        {
            Debug.LogError("Assign at least 3 color placeholders in the Inspector!");
            return;
        }

        placeholderColors = new Color[colorPlaceholders.Length];

        // Add button listeners
        redButton.onClick.AddListener(() => SetColor(redColor));
        blueButton.onClick.AddListener(() => SetColor(blueColor));
        yellowButton.onClick.AddListener(() => SetColor(yellowColor));
        greenButton.onClick.AddListener(() => SetColor(greenColor));
        purpleButton.onClick.AddListener(() => SetColor(purpleColor));
        clearButton.onClick.AddListener(ClearColors);

        // Set all placeholders to clear at start
    }

    void SetColor(Color color)
    {
        Debug.Log("Setting color: " + color);
        AudioManager.Instance.Play(clickSoundName);
        // Find the first empty placeholder and set its color
        for (int i = 0; i < colorPlaceholders.Length - 1; i++) // Exclude the result placeholder
        {
            if (placeholderColors[i] == Color.clear)
            {
                placeholderColors[i] = color;
                colorPlaceholders[i].color = color;
                Debug.Log("Color set at index: " + i);
                break;
            }
        }

        // Update the result in the last placeholder with a delay
        StopAllCoroutines();
        StartCoroutine(UpdateResultWithDelay());
    }

    IEnumerator UpdateResultWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Delay before showing the result

        if (placeholderColors[0] != Color.clear && placeholderColors[1] != Color.clear)
        {
            Color resultColor = MixColors(placeholderColors[0], placeholderColors[1]);
            placeholderColors[2] = resultColor;
            colorPlaceholders[2].color = resultColor;
            Debug.Log("Result color: " + resultColor);
        }
    }

    Color MixColors(Color color1, Color color2)
    {
        if ((color1 == redColor && color2 == yellowColor) || (color1 == yellowColor && color2 == redColor))
            return redYellow;
        if ((color1 == blueColor && color2 == yellowColor) || (color1 == yellowColor && color2 == blueColor))
            return blueYellow;
        if ((color1 == redColor && color2 == blueColor) || (color1 == blueColor && color2 == redColor))
            return redBlue;
        if ((color1 == blueColor && color2 == greenColor) || (color1 == greenColor && color2 == blueColor))
            return blueGreen;
        if ((color1 == blueColor && color2 == purpleColor) || (color1 == purpleColor && color2 == blueColor))
            return bluePurple;
        if ((color1 == redColor && color2 == greenColor) || (color1 == greenColor && color2 == redColor))
            return redGreen;
        if ((color1 == redColor && color2 == purpleColor) || (color1 == purpleColor && color2 == redColor))
            return redPurple;
        if ((color1 == yellowColor && color2 == greenColor) || (color1 == greenColor && color2 == yellowColor))
            return yellowGreen;
        if ((color1 == greenColor && color2 == purpleColor) || (color1 == purpleColor && color2 == greenColor))
            return greenPurple;
        if ((color1 == yellowColor && color2 == purpleColor) || (color1 == purpleColor && color2 == yellowColor))
            return yellowPurple;

        return Color.clear; // Return clear if no match
    }


    void ClearColors()
    {
        AudioManager.Instance.Play(clickSoundName);
        for (int i = 0; i < colorPlaceholders.Length; i++)
        {
            placeholderColors[i] = Color.clear;
            colorPlaceholders[i].color = Color.white; // White for visual clarity
        }
    }
}
