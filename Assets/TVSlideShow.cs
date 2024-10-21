using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Add this for TextMeshPro

public class TVSlideShow : MonoBehaviour
{
    public Image displayImage;         // Reference to the UI Image component on the canvas
    public Sprite[] slides;            // Array of images to display
    public Transform itemHolder;       // Reference to the player's item holder (where they hold objects)
    public GameObject remote;          // The remote object that the player needs to turn on the TV

    public KeypadNumbers keypadNumbers;
    private int currentRandomIndex = 0;  // Track the index of the current random slide
    private bool isTVOn = false;         // Track if the TV is on
    private Coroutine fadeCoroutine;     // Reference to handle fade coroutine
    private int[] randomIndices;         // Array to store the randomized slide order
    private bool[] hasShown;             // Track if a slide has been shown
    public AlertUI alertUI;
    public GameObject passwordClueDisplay; // Reference to the UI GameObject for the password clue
    public TextMeshProUGUI passwordText; // Reference to the TextMeshPro component for the password

    private void Start()
    {
        SetImageAlpha(0f);               // Start with TV off (invisible)
        InitializeRandomOrder();          // Set up the randomized order of slides
        passwordClueDisplay.SetActive(false); // Ensure the password clue is hidden at start
    }

    // Method to initialize the random slide order
    private void InitializeRandomOrder()
    {
        randomIndices = new int[slides.Length];
        hasShown = new bool[slides.Length];

        for (int i = 0; i < slides.Length; i++)
        {
            randomIndices[i] = i;
            hasShown[i] = false;
        }

        ShuffleSlides();  // Shuffle the order of the slides
    }

    // Shuffle the randomIndices array
    private void ShuffleSlides()
    {
        for (int i = 0; i < randomIndices.Length; i++)
        {
            int randomIndex = Random.Range(0, randomIndices.Length);
            int temp = randomIndices[i];
            randomIndices[i] = randomIndices[randomIndex];
            randomIndices[randomIndex] = temp;
        }
        currentRandomIndex = 0;
    }

    private void TurnOnTV()
    {
        isTVOn = true;
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine); // Stop any existing fade coroutine
        fadeCoroutine = StartCoroutine(FadeInTV()); // Start fade-in animation
    }

    private IEnumerator FadeInTV()
    {
        float duration = 1.5f; // Duration of the fade-in
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / duration); // Gradually increase alpha
            SetImageAlpha(alpha);
            yield return null;
        }
        SetImageAlpha(1f); // Ensure alpha is fully set to 1
        ShowNextSlide();   // Display the first random slide once fade is complete
    }

    public void ShowNextSlide()
    {
        if (slides.Length == 0) return; // Check if there are any slides

        // Mark the current slide as shown
        hasShown[randomIndices[currentRandomIndex]] = true;

        // Move to the next random index
        currentRandomIndex++;

        // If we've shown all slides, just reshuffle without showing the password clue
        if (currentRandomIndex >= randomIndices.Length)
        {
            InitializeRandomOrder(); // Reshuffle the slides when all are shown
            return; // Exit the function without showing the password clue
        }

        // Find the next slide to show that hasn't been shown yet
        while (hasShown[randomIndices[currentRandomIndex]])
        {
            currentRandomIndex++;
            if (currentRandomIndex >= randomIndices.Length)
            {
                InitializeRandomOrder(); // Reshuffle if all slides have been shown
                return; // Exit the function without showing the password clue
            }
        }

        UpdateSlideImage();
    }

    public void ShowPasswordClue()
    {
        passwordClueDisplay.SetActive(true); // Show the password clue UI
        GenerateRandomPassword(); // Generate and display the random password
        Debug.Log("All slides matched! Password clue displayed.");

        if (keypadNumbers != null)
        {
            keypadNumbers.SetKeypadPassword(passwordText.text); // Set the correct password for the keypad
        }
        else
        {
            Debug.LogError("KeypadNumbers reference is missing.");
        }
    }

    private void GenerateRandomPassword()
    {
        int password = Random.Range(10000, 99999); // Generate a random 5-digit number
        passwordText.text = password.ToString(); // Set the password text
    }

    // Display the current slide
    private void UpdateSlideImage()
    {
        int slideIndex = randomIndices[currentRandomIndex];
        displayImage.sprite = slides[slideIndex];
    }

    private bool HasRemote()
    {
        // Check if the remote is currently in the player's item holder
        return itemHolder.childCount > 0 && itemHolder.GetChild(0).gameObject == remote;
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = displayImage.color;
        color.a = alpha;
        displayImage.color = color;
    }

    public void InteractWithTV()
    {
        if (!isTVOn)
        {
            // Check if the player has the remote
            if (HasRemote())
            {
                TurnOnTV(); // Turn on the TV with fade-in effect
            }
            else
            {
                alertUI.ShowAlert("You need the remote to turn on the TV.");
                Debug.Log("You need the remote to turn on the TV.");
            }
        }
        else
        {
            // Check if the player still has the remote for moving to the next slide
            if (HasRemote())
            {
                ShowNextSlide(); // Move to the next random slide if the TV is already on
            }
            else
            {
                alertUI.ShowAlert("You need the remote to change slide");
                Debug.Log("You need the remote to change the slide.");
            }
        }
    }

    public Sprite GetCurrentSlide()
    {
        return slides[randomIndices[currentRandomIndex]];
    }
}
