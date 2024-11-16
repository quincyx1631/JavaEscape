using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswordPuzzle : MonoBehaviour
{
    public List<CrosswordGroup> crosswordGroups;  // List of crossword groups
    public List<GameObject> imagesToHighlight;    // List of images to highlight when completed
    public float delayBeforeHighlight = 1.5f;     // Delay before starting the highlight (activation + fade-in)
    public float fadeDuration = 1f;               // Duration for the fade-in effect

    private bool allGroupsComplete = false;       // Flag to check if all groups are complete

    private string allGroupsCompleteSound = "correct2"; // Sound for all groups completion
    private string revealCluesSound = "highlight";             // Sound for revealing clues

    private void Update()
    {
        bool tempAllGroupsComplete = true;  // Temporary flag to check if all groups are complete

        for (int i = 0; i < crosswordGroups.Count; i++)
        {
            var group = crosswordGroups[i];
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
            CollectionManager.Instance.MarkAsCollected(this.GetComponent<Interactables>());// Mark that all groups are complete
            AudioManager.Instance.Play(allGroupsCompleteSound);  // Play sound for all groups completion
            StartCoroutine(ActivateImagesAndFadeIn());
        }
    }

    private IEnumerator ActivateImagesAndFadeIn()
    {
        // Play sound effect for revealing clues
        AudioManager.Instance.Play(revealCluesSound);

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
    }
}
