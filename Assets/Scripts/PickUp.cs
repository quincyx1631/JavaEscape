using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform itemHolder;
    private Transform originalParent;

    private void Start()
    {
        // Ensure the item is active when in the world
        gameObject.SetActive(true);
        originalParent = transform.parent; // Store the original parent
    }

    public bool IsItemHolderEmpty()
    {
        // Check if the item holder has any children
        return itemHolder.childCount == 0;
    }

    public void pickUp()
    {
        if (itemHolder != null && IsItemHolderEmpty())
        {
            // Move the item to the item holder
            transform.SetParent(itemHolder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // Optionally, deactivate any unnecessary components or set the item to a different state
            // For example, you might want to disable the collider to prevent further interactions
            GetComponent<Collider>().enabled = false;
        }
    }

    public void Drop()
    {
        if (itemHolder != null && transform.parent == itemHolder)
        {
            // Move the item back to the original parent or set it to null to detach
            transform.SetParent(null);
            transform.position = itemHolder.position; // Optionally set the drop position
            transform.rotation = Quaternion.identity;

            // Reactivate necessary components
            GetComponent<Collider>().enabled = true;
        }
    }
}
