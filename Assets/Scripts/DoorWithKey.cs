using UnityEngine;
using System.Collections;

public class DoorWithKey : MonoBehaviour
{
    public Vector3 targetRotation = new Vector3(90f, 90f, -90f); // Desired final rotation angles
    public float rotationSpeed = 100f; // Speed of rotation
    public AlertUI alertUI; // Reference to the AlertUI component
    public string doorOpenSoundName; // Name of the door open sound in the Audio Manager
    public string doorLockedSoundName; // Name of the door locked sound in the Audio Manager
    public GameObject itemHolder; // The player's item holder GameObject
    public GameObject requiredKey; // The key GameObject required to unlock the door

    private bool isOpened = false; // Track if the door is already opened

    private void Start()
    {
        if (HasKey())
        {
            UnlockAndOpenDoor(); // Automatically unlock and open the door if the player has the key at the start
        }
    }

    private bool HasKey()
    {
        // Check if the required key is a child or reference within the item holder
        return requiredKey.transform.IsChildOf(itemHolder.transform);
    }

    public void AttemptOpen()
    {
        if (HasKey())
        {
            UnlockAndOpenDoor();
        }
        else
        {
            Debug.Log("Door is locked.");
            alertUI.ShowAlert("The door is locked.", doorLockedSoundName); // Show alert if the door is locked
        }
    }

    private void UnlockAndOpenDoor()
    {
        if (!isOpened)
        {
            StartCoroutine(OpenDoor()); // Unlock and immediately open the door
        }
    }

    public IEnumerator OpenDoor()
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
    }
}
