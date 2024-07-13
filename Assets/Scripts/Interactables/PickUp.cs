using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform itemHolder;
    private Transform originalParent;
    private Rigidbody rb;
    private Vector3 originalScale; // Store the original scale of the item

    private void Start()
    {
        // Ensure the item is active when in the world
        gameObject.SetActive(true);
        originalParent = transform.parent; // Store the original parent
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        originalScale = transform.localScale; // Store the original scale
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
            Debug.Log("Picking up item: " + gameObject.name);
            // Move the item to the item holder
            transform.SetParent(itemHolder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // Optionally, deactivate any unnecessary components or set the item to a different state
            // For example, you might want to disable the collider to prevent further interactions
            GetComponent<Collider>().enabled = false;

            // Disable Rigidbody to prevent physics interference
            rb.isKinematic = true;
        }
    }

    public void Drop()
    {
        if (itemHolder != null && transform.parent == itemHolder)
        {
            Debug.Log("Dropping item: " + gameObject.name);
            // Detach the item from the item holder
            transform.SetParent(null);

            // Reset the scale to its original scale
            transform.localScale = originalScale;

            // Optionally set the drop position and add some force for realistic dropping
            transform.position = itemHolder.position;
            transform.rotation = Quaternion.identity;

            // Reactivate necessary components
            GetComponent<Collider>().enabled = true;

            // Enable Rigidbody and apply gravity
            rb.isKinematic = false;

            // Get the forward direction of the player or item holder
            Vector3 forwardDirection = itemHolder.forward;

            // Apply a forward and downward force to simulate a toss
            Vector3 tossForce = forwardDirection * 5f + Vector3.down * 2f; // Adjust the multiplier to control the force strength
            rb.AddForce(tossForce, ForceMode.Impulse);

            // Optionally, add some torque to make the item spin
            Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rb.AddTorque(randomTorque * 10f, ForceMode.Impulse); // Adjust the multiplier to control the torque strength
        }
    }
}
