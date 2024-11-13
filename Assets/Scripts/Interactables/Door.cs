using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool isSliderDoor = false; // Set to true if the door is a slider

    public Vector3 targetRotation = new Vector3(90f, 90f, -90f); // Desired final rotation angles for rotating doors
    public Vector3 targetPosition; // Desired final position for sliding doors
    public float openSpeed = 1f; // Speed of rotation or sliding
    public GameObject finishUI; // Reference to the Finish UI GameObject
    public Timer timer; // Reference to the Timer component
    public float finalElapsedTime; // Variable to store the final elapsed time
    public AlertUI alertUI; // Reference to the AlertUI component
    public string doorOpenSoundName; // Name of the door open sound in the Audio Manager
    public string doorLockedSoundName; // Name of the door locked sound in the Audio Manager

    private bool isLocked = true; // Track if the door is locked
    private bool isOpened = false; // Track if the door is already opened

    private void Start()
    {
        isLocked = true;
        isOpened = false;

        if (finishUI != null)
        {
            finishUI.SetActive(false); // Ensure Finish UI is hidden initially
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

        if (isSliderDoor)
        {
            // Sliding door logic (only X-axis position changes)
            Vector3 initialPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                // Modify only the X-axis
                Vector3 newPosition = new Vector3(
                    Mathf.Lerp(initialPosition.x, targetPosition.x, elapsedTime), // Only interpolate the X position
                    initialPosition.y, // Keep the Y position unchanged
                    initialPosition.z  // Keep the Z position unchanged
                );

                transform.position = newPosition; // Update the position with only the X-axis change
                elapsedTime += Time.deltaTime * openSpeed;
                yield return null;
            }

            // Ensure the door ends exactly at the target position (only X-axis)
            transform.position = new Vector3(targetPosition.x, initialPosition.y, initialPosition.z);
        }
        else
        {
            // Rotating door logic
            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotationQuaternion, elapsedTime);
                elapsedTime += Time.deltaTime * openSpeed;
                yield return null;
            }

            transform.rotation = targetRotationQuaternion; // Ensure the door ends exactly at the target rotation
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
