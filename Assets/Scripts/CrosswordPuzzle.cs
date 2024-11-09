using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswordPuzzle : MonoBehaviour
{
    public List<CrosswordGroup> crosswordGroups;  // List of crossword groups
    public List<GameObject> imagesToHighlight;  // List of images to highlight when completed
    public float delayBeforeHighlight = 1.5f;   // Delay before starting the highlight (activation + fade-in)
    public float fadeDuration = 1f;              // Duration for the fade-in effect

    private bool allGroupsComplete = false;      // Flag to check if all groups are complete

    private void Update()
    {
        bool tempAllGroupsComplete = true;  // Temporary flag to check if all groups are complete

        // Loop through each group to check if it is complete
        for (int i = 0; i < crosswordGroups.Count; i++)
        {
            var group = crosswordGroups[i];
            if (group.IsGroupComplete())  // If the group is complete
            {
                group.LockGroup();  // Lock the group to prevent further input
            }
            else
            {
                tempAllGroupsComplete = false;  // Set flag to false if any group is incomplete
            }
        }

        // If all groups are complete, start activating and fading in images
        if (tempAllGroupsComplete && !allGroupsComplete)
        {
            allGroupsComplete = true;  // Mark that all groups are complete
            StartCoroutine(ActivateImagesAndFadeIn());
        }
    }

    private IEnumerator ActivateImagesAndFadeIn()
    {
        // First, set all images to active at once
        for (int i = 0; i < imagesToHighlight.Count; i++)
        {
            if (i < crosswordGroups.Count)
            {
                GameObject image = imagesToHighlight[i];
                image.SetActive(true);

                // Add CanvasGroup if it doesn't already exist
                CanvasGroup canvasGroup = image.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = image.AddComponent<CanvasGroup>();
                }

                // Set initial alpha to 0 (hidden)
                canvasGroup.alpha = 0f;
            }
        }

        // Wait for the specified delay before starting the fade-in
        yield return new WaitForSeconds(delayBeforeHighlight);

        // Fade all images in at the same time
        float startTime = Time.time;

        // Keep fading until all images are fully visible
        while (Time.time - startTime < fadeDuration)
        {
            foreach (var image in imagesToHighlight)
            {
                if (image.activeSelf)
                {
                    CanvasGroup canvasGroup = image.GetComponent<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeDuration);
                    }
                }
            }
            yield return null;
        }

        // Ensure all images are fully faded in
        foreach (var image in imagesToHighlight)
        {
            if (image.activeSelf)
            {
                CanvasGroup canvasGroup = image.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f;
                }
            }
        }
    }
}
