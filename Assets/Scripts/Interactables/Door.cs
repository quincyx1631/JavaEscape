using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public GameObject finishUI; // Reference to the Finish UI GameObject
    public Timer timer; // Reference to the Timer component
    public float finalElapsedTime; // Variable to store the final elapsed time
    public AlertUI alertUI; // Reference to the AlertUI component
    public string doorOpenSoundName; // Name of the door open sound in the Audio Manager
    public string doorLockedSoundName; // Name of the door locked sound in the Audio Manager

    private bool isLocked = true; // Track if the door is locked
    private bool isOpened = false; // Track if the door is already opened

    public Animator doorAnimator; // Reference to the door's Animator component

    private void Start()
    {
        isLocked = true;
        isOpened = false;

        if (finishUI != null)
        {
            finishUI.SetActive(false); // Ensure Finish UI is hidden initially
        }

        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>(); // Get the Animator component if not already set
        }
    }

    public void AttemptOpen()
    {
        if (!isLocked)
        {
            StartCoroutine(OpenDoor()); // If unlocked, proceed to open the door
        }
        else
        {
            Debug.Log("Door is locked.");
            alertUI.ShowAlert("The door is locked.", doorLockedSoundName); // Show alert if the door is locked
        }
    }

    public void UnlockDoor()
    {
        if (!isLocked)
        {
            Debug.Log("Door is already unlocked.");
            return;
        }

        isLocked = false;
        Debug.Log("Door unlocked.");
    }

    public IEnumerator OpenDoor()
    {
        if (isOpened) yield break;

        // Play the door open sound
        if (!string.IsNullOrEmpty(doorOpenSoundName))
        {
            AudioManager.Instance.Play(doorOpenSoundName);
        }

        // Trigger the animation to open the door
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenDoor");
        }

        isOpened = true;

        // Stop the timer and store the elapsed time
        if (timer != null)
        {
            timer.StopTimer();
            finalElapsedTime = timer.GetElapsedTime();
        }

        // Wait 1.5 seconds before showing the Finish UI
        yield return new WaitForSeconds(1.5f);

        if (finishUI != null)
        {
            FinishUI.Instance.DisplayFinalTime(finalElapsedTime); // Display the final time on the Finish UI
            finishUI.SetActive(true); // Display the Finish UI
        }
    }
}
