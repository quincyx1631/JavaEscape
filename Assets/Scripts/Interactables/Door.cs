using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public Vector3 targetRotation = new Vector3(90f, 90f, -90f); // Desired final rotation angles
    public float rotationSpeed = 100f; // Speed of rotation
    public GameObject finishUI; // Reference to the Finish UI GameObject
    public Timer timer; // Reference to the Timer component
    private float finalElapsedTime; // Variable to store the final elapsed time
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

    private IEnumerator OpenDoor()
    {
        if (isOpened) yield break;

        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);
        float elapsedTime = 0f;

        Debug.Log($"Opening Door: Initial Rotation: {initialRotation.eulerAngles}, Target Rotation: {targetRotationQuaternion.eulerAngles}");

        // Play the door open sound
        if (!string.IsNullOrEmpty(doorOpenSoundName))
        {
            AudioManager.Instance.Play(doorOpenSoundName);
        }

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotationQuaternion, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        // Ensure the door ends exactly at the target rotation
        transform.rotation = targetRotationQuaternion;
        isOpened = true;
        Debug.Log($"Door opened: Final Rotation: {transform.rotation.eulerAngles}");

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

        //Saving Progress Script
        UnlockNewLevel();
    }

    void UnlockNewLevel()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex >= PlayerPrefs.GetInt("ReachIndex"))
        {
            PlayerPrefs.SetInt("ReachIndex", currentSceneIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();

            Debug.Log("Level unlocked: ReachIndex updated to " + (currentSceneIndex + 1));
        }
    }
}
