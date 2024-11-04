using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LaptopSlideShow : MonoBehaviour
{
    public Image displayImage;         // Reference to the UI Image component
    public Slide[] slides;             // Array of Slide objects
    public TVSlideShow tvSlideShow;    // Reference to the TV slideshow script
    public AlertUI alertUI;            // Reference to the AlertUI system

    private int currentSlideIndex = 0; // Index of the current slide
    private bool[] matchedSlides;       // To track matched slides

    private void Start()
    {
        matchedSlides = new bool[slides.Length];
        if (slides.Length > 0)
        {
            UpdateSlideImage();
        }
        else
        {
            Debug.LogWarning("No slides available to display.");
        }
    }

    public void ShowNextSlide()
    {
        if (slides.Length == 0) return;

        currentSlideIndex = (currentSlideIndex + 1) % slides.Length;
        UpdateSlideImage();
    }

    public void ShowPreviousSlide()
    {
        if (slides.Length == 0) return;

        currentSlideIndex = (currentSlideIndex - 1 + slides.Length) % slides.Length;
        UpdateSlideImage();
    }

    public void CheckIfSlidesMatch()
    {
        if (tvSlideShow != null)
        {
            if (!tvSlideShow.IsTVOn())
            {
                alertUI.ShowAlert("Please turn on the TV first.");
                return;
            }

            if (matchedSlides[currentSlideIndex])
            {
                alertUI.ShowAlert("This slide has already been matched.");
                return;
            }

            // Compare slide titles instead of images
            if (slides[currentSlideIndex].slideTitle == tvSlideShow.GetCurrentSlideTitle())
            {
                alertUI.ShowAlert("Correct Match!");
                matchedSlides[currentSlideIndex] = true;
                ApplyGrayscaleEffect();

                if (AllSlidesMatched())
                {
                    tvSlideShow.ShowPasswordClue();

                    // Mark as collected when all slides are matched
                    CollectionManager.Instance.MarkAsCollected(this.GetComponent<Interactables>());
                }
            }
            else
            {
                alertUI.ShowAlert("Wrong Code Match");
            }
        }
    }

    private void ApplyGrayscaleEffect()
    {
        displayImage.color = new Color(0.3f, 0.3f, 0.3f);
    }

    private void UpdateSlideImage()
    {
        displayImage.sprite = slides[currentSlideIndex].slideImage; // Accessing slideImage correctly
        displayImage.color = matchedSlides[currentSlideIndex] ? new Color(0.3f, 0.3f, 0.3f) : Color.white;
    }

    private bool AllSlidesMatched()
    {
        foreach (bool matched in matchedSlides)
        {
            if (!matched) return false;
        }
        return true;
    }
}
