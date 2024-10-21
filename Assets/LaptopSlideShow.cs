using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this for TextMeshPro

public class LaptopSlideShow : MonoBehaviour
{
    public Image displayImage;           // Reference to the UI Image component on the canvas
    public Sprite[] slides;              // Array of images to display
    public TVSlideShow tvSlideShow;      // Reference to the TV slideshow script
    public AlertUI alertUI;              // Reference to the AlertUI system for showing alerts

    private int currentSlideIndex = 0;   // Index of the current slide
    private bool[] matchedSlides;         // To track which slides have been matched

    private void Start()
    {
        matchedSlides = new bool[slides.Length];  // Initialize matched slide tracking
    }

    // Show the next slide when pressing the "Next Slide" button
    public void ShowNextSlide()
    {
        if (slides.Length == 0) return;

        currentSlideIndex++;

        if (currentSlideIndex >= slides.Length)
        {
            currentSlideIndex = 0; // Loop back to the first slide
        }

        UpdateSlideImage();
    }

    // Show the previous slide when pressing the "Previous Slide" button
    public void ShowPreviousSlide()
    {
        if (slides.Length == 0) return;

        currentSlideIndex--;

        if (currentSlideIndex < 0)
        {
            currentSlideIndex = slides.Length - 1; // Loop back to the last slide
        }

        UpdateSlideImage();
    }

    // Method to check if the current laptop slide matches the current TV slide
    public void CheckIfSlidesMatch()
    {
        if (tvSlideShow != null)
        {
            // Check if the current slide has already been matched
            if (matchedSlides[currentSlideIndex])
            {
                alertUI.ShowAlert("This slide has already been matched.");
                Debug.Log("This slide has already been matched.");
                return;
            }

            // Compare the laptop's current slide sprite with the TV's current slide sprite
            if (slides[currentSlideIndex] == tvSlideShow.GetCurrentSlide())
            {
                Debug.Log("The slides match!");
                alertUI.ShowAlert("Correct Match!");

                // Mark this slide as matched
                matchedSlides[currentSlideIndex] = true;

                // Apply a black and white effect (grayscale) to the matched slide
                ApplyGrayscaleEffect();

                // Check if all slides have been matched
                if (AllSlidesMatched())
                {
                    tvSlideShow.ShowPasswordClue(); // Call to show the password clue from TVSlideShow
                }
            }
            else
            {
                alertUI.ShowAlert("Wrong Code Match");
                Debug.Log("The slides do not match.");
            }
        }
    }

    // Apply a grayscale effect to the matched slide
    private void ApplyGrayscaleEffect()
    {
        Color grayscaleColor = new Color(0.3f, 0.3f, 0.3f); // Grayscale color
        displayImage.color = grayscaleColor;  // Change the color to simulate black and white
    }

    // Method to update the displayed slide image and reset color if not matched
    private void UpdateSlideImage()
    {
        displayImage.sprite = slides[currentSlideIndex];

        // Apply grayscale only if the slide has been matched
        if (matchedSlides[currentSlideIndex])
        {
            ApplyGrayscaleEffect();  // Apply grayscale effect if already matched
        }
        else
        {
            displayImage.color = Color.white;  // Keep the normal color for unmatched slides
        }
    }

    // Method to check if all slides have been matched
    private bool AllSlidesMatched()
    {
        foreach (bool matched in matchedSlides)
        {
            if (!matched) return false; // If any slide is not matched, return false
        }
        return true; // All slides have been matched
    }
}
