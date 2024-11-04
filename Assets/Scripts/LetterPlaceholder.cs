using UnityEngine;
using System.Collections;

public class LetterPlaceholder : MonoBehaviour
{
    public Transform placeholder; // Position where the cube should land
    public Transform playerItemHolder; // Position where the item is held by the player
    private GameObject placedItem; // To keep track of the placed item
    private bool isOccupied; // To track if the placeholder is currently occupied
    private string originalTag; // To store the original tag of the item
    public Light placeholderLight; // Public Light for changing the color
    public Color correctColor = Color.green; // Light color for correct letter
    public Color incorrectColor = Color.red; // Light color for incorrect letter
    private Color originalLightColor; // Original color of the light
    public AlertUI alertUI;
    [SerializeField] private char letter; // The letter that this placeholder accepts

    private void Awake()
    {
        if (placeholderLight != null)
        {
            originalLightColor = placeholderLight.color;
            placeholderLight.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LetterBox>() != null)
        {
            isOccupied = true;
            placedItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<LetterBox>() != null && other.gameObject == placedItem)
        {
            placedItem = null;
            isOccupied = false;
            ResetLightColor();
        }
    }

    public void InteractPlaceholder()
    {
        if (isOccupied && placedItem != null)
        {
            RemoveItem();
        }
        else
        {
            PlaceItem();
        }
    }

    private void PlaceItem()
    {
        // Ensure there is an item in the player's ItemHolder and it has a LetterBox component
        if (playerItemHolder.childCount > 0)
        {
            GameObject itemToPlace = playerItemHolder.GetChild(0).gameObject;
            LetterBox letterBoxScript = itemToPlace.GetComponent<LetterBox>();

            if (letterBoxScript != null)
            {
                originalTag = itemToPlace.tag;

                itemToPlace.transform.SetParent(null);
                itemToPlace.layer = LayerMask.NameToLayer("Default");
                itemToPlace.transform.position = placeholder.position + Vector3.up * 0.5f;
                itemToPlace.transform.localScale = letterBoxScript.GetOriginalScale();
                float randomYRotation = Random.Range(0f, 360f);
                itemToPlace.transform.rotation = Quaternion.Euler(0, randomYRotation, 0);

                Collider itemCollider = itemToPlace.GetComponent<Collider>();
                if (itemCollider != null)
                {
                    itemCollider.enabled = true;
                }

                Rigidbody itemRigidbody = itemToPlace.GetComponent<Rigidbody>();
                if (itemRigidbody == null)
                {
                    itemRigidbody = itemToPlace.AddComponent<Rigidbody>();
                }
                itemRigidbody.isKinematic = false;

                itemToPlace.tag = "Untagged";

                if (letterBoxScript.letter == letter)
                {
                    StartCoroutine(DelayedLightChange(correctColor));
                }
                else
                {
                    StartCoroutine(DelayedLightChange(incorrectColor));
                }

                placedItem = itemToPlace;
                isOccupied = true;

                Debug.Log("Item placed on placeholder.");
            }
            else
            {
                alertUI.ShowAlert("Invalid Item", "Alert");
                Debug.Log("Cannot place item: Item does not have a LetterBox component.");
            }
        }
        else
        {
            alertUI.ShowAlert("No item to place", "Alert");
            Debug.Log("No item to place.");
        }
    }

    private IEnumerator DelayedLightChange(Color color)
    {
        yield return new WaitForSeconds(0.3f);
        ChangeLightColor(color);
    }

    private void RemoveItem()
    {
        // Check if the player's ItemHolder is empty before allowing item removal
        if (playerItemHolder.childCount == 0)
        {
            if (placedItem != null)
            {
                placedItem.transform.SetParent(playerItemHolder);
                placedItem.transform.localPosition = Vector3.zero;
                placedItem.transform.rotation = Quaternion.identity;

                Rigidbody itemRigidbody = placedItem.GetComponent<Rigidbody>();
                if (itemRigidbody != null)
                {
                    itemRigidbody.isKinematic = true;
                }

                // Set the layer to "Item" for re-detection
                placedItem.layer = LayerMask.NameToLayer("Item"); // Ensure it's on the "Item" layer
                Collider itemCollider = placedItem.GetComponent<Collider>();
                if (itemCollider != null)
                {
                    itemCollider.enabled = true; // Re-enable collider for detection
                }

                placedItem.tag = originalTag; // Restore original tag if needed
                placedItem = null;
                isOccupied = false;
                ResetLightColor();

                Debug.Log("Item removed from placeholder and returned to ItemHolder with layer 'Item'.");
            }
        }
        else
        {
            alertUI.ShowAlert("Drop the item you are holding first", "Alert");
            Debug.Log("Cannot remove item: Player's ItemHolder is already occupied.");
        }
    }



    private void ChangeLightColor(Color color)
    {
        if (placeholderLight != null)
        {
            placeholderLight.gameObject.SetActive(true);
            placeholderLight.color = color;
        }
    }

    private void ResetLightColor()
    {
        if (placeholderLight != null)
        {
            placeholderLight.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (placedItem != null && !placedItem.activeInHierarchy)
        {
            placedItem = null;
            isOccupied = false;
            ResetLightColor();
            Debug.Log("Item removed from placeholder. Ready to place a new item.");
        }
    }

    public void SetLetter(char newLetter)
    {
        letter = newLetter;
    }

    public bool HasCorrectLetter()
    {
        if (placedItem != null)
        {
            LetterBox letterBoxScript = placedItem.GetComponent<LetterBox>();
            return letterBoxScript != null && letterBoxScript.letter == letter;
        }
        return false;
    }

    public void SetToUntagged()
    {
        tag = "Untagged";
    }

    public GameObject GetPlacedItem()
    {
        return placedItem;
    }
}
