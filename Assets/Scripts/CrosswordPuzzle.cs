using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswordPuzzle : MonoBehaviour
{
    public List<CrosswordGroup> crosswordGroups;  // List of crossword groups
    public List<GameObject> imagesToHighlight;   // List of images to highlight when completed
    public List<GameObject> boxButtons;          // List of "Boxes Button" objects
    public float delayBeforeHighlight = 1.5f;    // Delay before starting the highlight
    public float fadeDuration = 1f;              // Duration for the fade-in effect

    private bool allGroupsComplete = false;      // Flag to check if all groups are complete

    private string allGroupsCompleteSound = "highlight"; // Sound for all groups completion
    private string revealCluesSound = "FinishPuzzle";      // Sound for revealing clues

    private void Start()
    {
        // Initially tag all box buttons as "Untagged"
        foreach (var button in boxButtons)
        {
            button.tag = "Untagged";
        }
    }

    private void Update()
    {
        bool tempAllGroupsComplete = true;  // Temporary flag to check if all groups are complete

        foreach (var group in crosswordGroups)
        {
            if (group.IsGroupComplete())  // If the group is complete
            {
                group.LockGroup();        // Lock the group to prevent further input
            }
            else
            {
                tempAllGroupsComplete = false;  // Set flag to false if any group is incomplete
            }
        }

        // If all groups are complete, start activating and fading in images
        if (tempAllGroupsComplete && !allGroupsComplete)
        {
            allGroupsComplete = true;
            CollectionManager.Instance.MarkAsCollected(this.GetComponent<Interactables>()); // Mark that all groups are complete
            AudioManager.Instance.Play(allGroupsCompleteSound);  // Play sound for all groups completion
            StartCoroutine(ActivateImagesAndFadeIn());
        }
    }

    private IEnumerator ActivateImagesAndFadeIn()
    {
        // Play sound effect for revealing clues
        AudioManager.Instance.Play(revealCluesSound);

        foreach (var image in imagesToHighlight)
        {
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

        yield return new WaitForSeconds(delayBeforeHighlight);

        float startTime = Time.time;

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

        // After the fade-in is complete, tag all box buttons as "Interactables"
        foreach (var button in boxButtons)
        {
            button.tag = "Interactables";
        }
    }
}
