using UnityEngine;

public class LetterPlaceholder : MonoBehaviour
{
    public Transform placeholder; // Position where the cube should land
    public Transform playerItemHolder; // Position where the item is held by the player
    private GameObject placedItem; // To keep track of the placed item
    private bool isOccupied; // To track if the placeholder is currently occupied
    private string originalTag; // To store the original tag of the item
    private Renderer placeholderRenderer; // Renderer for changing the color
    public Color correctColor = Color.green; // Color for correct letter
    public Color incorrectColor = Color.red; // Color for incorrect letter
    private Color originalColor; // Original color of the placeholder

    [SerializeField] private char letter; // The letter that this placeholder accepts

    private void Awake()
    {
        // Get the Renderer component and store the original color
        placeholderRenderer = GetComponent<Renderer>();
        originalColor = placeholderRenderer.material.color; // Store the original color
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is a LetterBox
        if (other.GetComponent<LetterBox>() != null)
        {
            isOccupied = true; // Mark as occupied when an item is placed
            placedItem = other.gameObject; // Keep track of the placed item
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is a LetterBox
        if (other.GetComponent<LetterBox>() != null)
        {
            // Reset the placed item if it exits
            if (other.gameObject == placedItem)
            {
                placedItem = null; // Reset the placed item reference
            }
            isOccupied = false; // Mark as not occupied
            ResetColor(); // Reset color when the item is removed
        }
    }

    // This function will be called to interact with the placeholder
    public void InteractPlaceholder()
    {
        if (isOccupied && placedItem != null) // If an item is already placed
        {
            RemoveItem(); // Remove the existing item
        }
        else // If no item is placed
        {
            PlaceItem(); // Place a new item
        }
    }

    // Place a new item on the placeholder
    private void PlaceItem()
    {
        // Check if the player is holding any item at the ItemHolder position
        if (playerItemHolder.childCount > 0)
        {
            // Get the first child object (the cube) from the player's ItemHolder
            GameObject itemToPlace = playerItemHolder.GetChild(0).gameObject;
            LetterBox letterBoxScript = itemToPlace.GetComponent<LetterBox>();

            if (letterBoxScript != null)
            {
                // Store the original tag before changing it
                originalTag = itemToPlace.tag;

                // Detach the item from the player's ItemHolder without setting a new parent
                itemToPlace.transform.SetParent(null);

                // Change the item's layer to "Default"
                itemToPlace.layer = LayerMask.NameToLayer("Default");

                // Restore the original scale of the item
                itemToPlace.transform.localScale = letterBoxScript.GetOriginalScale();

                // Reset the position to just above the placeholder
                itemToPlace.transform.position = placeholder.position + Vector3.up * 0.5f;

                // Randomize the Y rotation
                float randomYRotation = Random.Range(0f, 360f);
                itemToPlace.transform.rotation = Quaternion.Euler(0, randomYRotation, 0);

                // Enable the collider of the item
                Collider itemCollider = itemToPlace.GetComponent<Collider>();
                if (itemCollider != null)
                {
                    itemCollider.enabled = true; // Enable the collider when placed
                }

                // Add a Rigidbody to the item if it doesn't have one
                Rigidbody itemRigidbody = itemToPlace.GetComponent<Rigidbody>();
                if (itemRigidbody == null)
                {
                    itemRigidbody = itemToPlace.AddComponent<Rigidbody>(); // Add Rigidbody component if missing
                }

                // Set the Rigidbody to Kinematic to control the drop
                itemRigidbody.isKinematic = true; // Disable physics temporarily

                // Allow the Rigidbody to drop
                itemRigidbody.isKinematic = false; // Enable physics to let it drop

                // Untag the item to prevent interaction
                itemToPlace.tag = "Untagged";

                // Check if the letter matches
                if (letterBoxScript.letter == letter) // Compare with the letter in this placeholder
                {
                    placeholderRenderer.material.color = correctColor; // Change to green
                }
                else
                {
                    placeholderRenderer.material.color = incorrectColor; // Change to red
                }

                // Keep track of the placed item
                placedItem = itemToPlace;
                isOccupied = true; // Mark as occupied

                Debug.Log("Item placed on placeholder.");
            }
            else
            {
                Debug.Log("Item does not have a LetterBox component.");
            }
        }
        else
        {
            Debug.Log("No item to place.");
        }
    }

    // Remove the item from the placeholder and return it to the player's item holder
    private void RemoveItem()
    {
        if (placedItem != null)
        {
            // Reset the item's position and parent
            placedItem.transform.SetParent(playerItemHolder); // Parent back to the player ItemHolder
            placedItem.transform.localPosition = Vector3.zero; // Reset local position
            placedItem.transform.rotation = Quaternion.identity; // Reset rotation

            // Optionally disable the Rigidbody to prevent unwanted physics interactions
            Rigidbody itemRigidbody = placedItem.GetComponent<Rigidbody>();
            if (itemRigidbody != null)
            {
                itemRigidbody.isKinematic = true; // Set to kinematic
            }

            // Restore the original tag
            placedItem.tag = originalTag; // Restore the original tag

            placedItem = null; // Clear the placed item reference
            isOccupied = false; // Mark as not occupied
            ResetColor(); // Reset color when the item is removed

            Debug.Log("Item removed from placeholder and returned to ItemHolder.");
        }
        else
        {
            Debug.Log("No item to remove.");
        }
    }

    private void ResetColor()
    {
        // Reset the color to the original when no item is placed
        placeholderRenderer.material.color = originalColor;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the placed item is no longer active in the scene
        if (placedItem != null && !placedItem.activeInHierarchy)
        {
            placedItem = null; // Allow placing a new item if the old one is gone
            isOccupied = false; // Also mark as not occupied since the item is gone
            ResetColor(); // Reset color when the item is removed
            Debug.Log("Item removed from placeholder. Ready to place a new item.");
        }
    }

    // Method to set the letter for this placeholder
    public void SetLetter(char newLetter)
    {
        letter = newLetter;
    }
    // Inside LetterPlaceholder class

    // Public method to check if the placed letter is correct
    public bool HasCorrectLetter()
    {
        if (placedItem != null)
        {
            LetterBox letterBoxScript = placedItem.GetComponent<LetterBox>();
            if (letterBoxScript != null)
            {
                return letterBoxScript.letter == letter; // Compare with the letter in this placeholder
            }
        }
        return false; // Return false if no item is placed
    }

    // Method to set the placeholder and its item to untagged
    public void SetToUntagged()
    {
        tag = "Untagged"; // Set the placeholder to untagged
    }
    public GameObject GetPlacedItem()
    {
        return placedItem; // Return the currently placed LetterBox
    }

}
