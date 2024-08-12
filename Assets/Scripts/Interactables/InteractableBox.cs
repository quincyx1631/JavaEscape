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

    void Start()
    {
        initialPosition = transform.position;
        // Calculate moved position in local space (forward direction)
        localMovedPosition = new Vector3(0, 0, 1.2f);
        interactables = GetComponent<Interactables>();
        UpdateMessage();  // Update the initial message based on the initial state

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
            HUDController.instance.EnableInteractionText("You need a key to open this box.");
            return;
        }

        Debug.Log("Interact method called");

        isOpen = !isOpen;
        Vector3 targetPosition = isOpen ? transform.TransformPoint(localMovedPosition) : initialPosition;
        StartCoroutine(MoveToPosition(targetPosition));

        UpdateMessage();  // Update the message whenever the object is interacted with
        HUDController.instance.EnableInteractionText(interactables.message);  // Update the HUD with the new message

        if (isOpen && requiresKey)
        {
            RemoveKey();  // Remove the key from the player's hand if the box requires a key
            requiresKey = false;  // Disable the key requirement for future interactions
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
        // Remove the key from the itemHolder
        foreach (Transform child in itemHolder)
        {
            if (child.gameObject == key)
            {
                Destroy(child.gameObject);
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

    private void UpdateMessage()
    {
        if (isOpen)
        {
            interactables.message = "[F] Close";
        }
        else
        {
            interactables.message = requiresKey ? "[F] Open (Key Required)" : "[F] Open";
        }
    }

    private void OnMouseEnter()
    {
        if (!isMoving)
        {
            HUDController.instance.EnableInteractionText(interactables.message);
        }
    }

    private void OnMouseExit()
    {
        HUDController.instance.DisableInteractionText();
    }
}
