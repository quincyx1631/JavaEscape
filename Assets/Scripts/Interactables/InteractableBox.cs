using System.Collections;
using UnityEngine;

public class InteractableBox : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 localMovedPosition;
    private bool isOpen = false;
    private bool isMoving = false;
    private Interactables interactables;
    public float moveDuration = 0.1f;  // Duration of the move in seconds
    public bool requiresKey = false;  // Boolean to determine if the box needs a key
    public GameObject key;  // Reference to the key GameObject (used if requiresKey is true)
    public Transform itemHolder;  // Reference to the itemHolder in the player
    public AlertUI alertUI; // Reference to the AlertUI script

    public AudioSource openSoundSource;  // AudioSource for the opening sound
    public AudioSource closeSoundSource; // AudioSource for the closing sound

    void Start()
    {
        initialPosition = transform.position;
        // Calculate moved position in local space (forward direction)
        localMovedPosition = new Vector3(0, 0, 1.2f);
        interactables = GetComponent<Interactables>();

        Debug.Log($"Initial Position: {initialPosition}");
    }

    public void Interact()
    {
        if (isMoving)
        {
            return;  // Do nothing if the box is already moving
        }

        if (requiresKey && !HasKey())
        {
            Debug.Log("You need a key to open this box.");
            alertUI.ShowAlert("You need a key on hand", "LockedDrawer");
            return;
        }

        Debug.Log("Interact method called");

        isOpen = !isOpen;
        Vector3 targetPosition = isOpen ? transform.TransformPoint(localMovedPosition) : initialPosition;

        // Play the appropriate sound based on whether the box is opening or closing
        PlaySound(isOpen ? openSoundSource : closeSoundSource);

        StartCoroutine(MoveToPosition(targetPosition));

        // Update the message whenever the object is interacted with

        if (isOpen && requiresKey)
        {
            RemoveKey();  // Remove the key from the player's hand if the box requires a key
            requiresKey = false;  // Disable the key requirement for future interactions
        }
    }

    private void PlaySound(AudioSource soundSource)
    {
        if (soundSource != null)
        {
            soundSource.Play(); // Play the assigned AudioSource
        }
        else
        {
            Debug.LogWarning("AudioSource is not assigned.");
        }
    }

    private bool HasKey()
    {
        // Check if the itemHolder has the key
        foreach (Transform child in itemHolder)
        {
            if (child.gameObject == key)
            {
                return true;
            }
        }
        return false;
    }

    private void RemoveKey()
    {
        // Find the key in the itemHolder and remove it from the player's hand without destroying it
        foreach (Transform child in itemHolder)
        {
            if (child.gameObject == key)
            {
                // Deactivate the key and move it to a hidden inventory or a predefined hidden location
                child.gameObject.SetActive(false);
                child.SetParent(null);  // Detach the key from the itemHolder
                break;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        float elapsedTime = 0;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;

        Debug.Log($"Box is now at position: {transform.position}");
    }
}
