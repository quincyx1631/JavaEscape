using UnityEngine;
using UnityEngine.UI;

public class LaptopSlideShow : MonoBehaviour
{
    public Image displayImage;  // Reference to the UI Image component on the canvas
    public Sprite[] slides;     // Array of images to display
    private int currentSlideIndex = 0;

    // Show the next slide when pressing the "Next Slide" button
    public void ShowNextSlide()
    {
        if (slides.Length == 0) return;

        currentSlideIndex++;

        if (currentSlideIndex >= slides.Length)
        {
            currentSlideIndex = 0; // Loop back to the first slide
        }

        displayImage.sprite = slides[currentSlideIndex];
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

        displayImage.sprite = slides[currentSlideIndex];
    }
}
