using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TVSlideShow : MonoBehaviour
{
    public Image displayImage;     // Reference to the UI Image component on the canvas
    public Sprite[] slides;        // Array of images to display
    public Transform itemHolder;   // Reference to the player's item holder (where they hold objects)
    public GameObject remote;      // The remote object that the player needs to turn on the TV

    private int currentSlideIndex = 0;
    private bool isTVOn = false;   // Track if the TV is on
    private Coroutine fadeCoroutine; // Reference to handle fade coroutine
    public AlertUI alertUI;
    private void Start()
    {
        // Set the image alpha to 0 (invisible) to represent the TV being off
        SetImageAlpha(0f);
    }

    // Method to turn on the TV (initiates fade-in)
    private void TurnOnTV()
    {
        isTVOn = true;
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine); // Stop any existing fade coroutine
        fadeCoroutine = StartCoroutine(FadeInTV()); // Start fade-in animation
    }

    // Coroutine to handle the fade-in effect
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
        ShowNextSlide();   // Display the first slide once the fade is complete
    }

    // Method to display the next slide
    public void ShowNextSlide()
    {
        if (slides.Length == 0) return; // Check if there are any slides
        currentSlideIndex++;  // Move to the next slide
        if (currentSlideIndex >= slides.Length) // Loop back to the first slide if needed
        {
            currentSlideIndex = 0;
        }

        displayImage.sprite = slides[currentSlideIndex]; // Set the next image
    }

    // Check if the player has the remote in the item holder
    private bool HasRemote()
    {
        // Check if the remote is currently in the player's item holder
        return itemHolder.childCount > 0 && itemHolder.GetChild(0).gameObject == remote;
    }

    // Method to adjust the alpha of the image
    private void SetImageAlpha(float alpha)
    {
        Color color = displayImage.color;
        color.a = alpha;
        displayImage.color = color;
    }

    // Called when the player interacts with the TV (combined open and next slide)
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
                ShowNextSlide(); // Move to the next slide if the TV is already on
            }
            else
            {
                alertUI.ShowAlert("You need the remote to change slide");
                Debug.Log("You need the remote to change the slide.");
            }
        }
    }
}
