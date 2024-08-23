using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform itemHolder;
    public string pickupSoundName;
    public string dropSoundName;
    public float dropForce = 5f;
    public float dropTorque = 10f;
    public Vector3 checkBoxSize = new Vector3(0.5f, 0.5f, 0.5f); // Size of the box for collision checking
    public Vector3 checkBoxCenter = Vector3.zero; // Center of the box for collision checking
    public Collider playerCollider; // Reference to the player's collider
    public string itemLayerName = "Item"; // Layer name for the item

    private Rigidbody rb;
    private Collider itemCollider;
    private bool canDropItem = true; // Flag to enable or disable dropping
    private int originalLayer; // Store the original layer
    private Vector3 originalScale; // Store the original scale

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>(); // Get the Collider component on start

        // Ensure MeshCollider is convex if present
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.convex = true;
        }
    }

    private void Update()
    {
        // Check for collisions continuously and update the drop status
        if (IsCollidingWithOtherObject())
        {
            DisableDrop();
        }
        else
        {
            EnableDrop();
        }

        // Handle dropping the item when the "G" key is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            TryDropItem();
        }
    }

    public void PickUpItem()
    {
        if (itemHolder != null && itemHolder.childCount == 0)
        {
            Debug.Log("Picking up item: " + gameObject.name);

            if (!string.IsNullOrEmpty(pickupSoundName))
            {
                AudioManager.Instance.Play(pickupSoundName);
            }

            // Store the original layer and scale
            originalLayer = gameObject.layer;
            originalScale = transform.localScale;

            // Change layer to "Item"
            SetLayer(itemLayerName);

            // Reset scale to the original scale
            transform.localScale = originalScale;

            transform.SetParent(itemHolder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // Disable the collider when picked up
            if (itemCollider != null)
            {
                itemCollider.enabled = false;
            }
            rb.isKinematic = true;
        }
    }

    public void Drop()
    {
        if (itemHolder != null && transform.parent == itemHolder && canDropItem)
        {
            Debug.Log("Dropping item: " + gameObject.name);

            // Restore original layer
            SetLayer(LayerMask.LayerToName(originalLayer));

            transform.SetParent(null);
            rb.isKinematic = false;

            // Re-enable the collider when dropped
            if (itemCollider != null)
            {
                itemCollider.enabled = true;
            }

            // Reset scale to the original scale
            transform.localScale = originalScale;

            // Add force and torque
            if (Camera.main != null)
            {
                Vector3 forwardDirection = Camera.main.transform.forward;
                rb.AddForce(forwardDirection * dropForce + Vector3.up * 2f, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * dropTorque, ForceMode.Impulse);
            }

            // Play drop sound on collision
            if (!string.IsNullOrEmpty(dropSoundName))
            {
                AudioManager.Instance.Play(dropSoundName);
            }
        }
    }

    private void TryDropItem()
    {
        if (canDropItem)
        {
            Drop();
        }
        else
        {
            Debug.Log("Item cannot be dropped because it's colliding with another object.");
        }
    }

    private bool IsCollidingWithOtherObject()
    {
        Vector3 worldCenter = transform.position + transform.TransformDirection(checkBoxCenter);
        Collider[] colliders = Physics.OverlapBox(worldCenter, checkBoxSize / 2);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider != playerCollider)
            {
                return true;
            }
        }
        return false;
    }

    private void EnableDrop()
    {
        canDropItem = true;
        Debug.Log("Drop enabled");
    }

    private void DisableDrop()
    {
        canDropItem = false;
        Debug.Log("Drop disabled");
    }

    private void SetLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1)
        {
            Debug.LogError($"Layer '{layerName}' does not exist. Please add this layer in the Unity Editor.");
        }
        else
        {
            gameObject.layer = layer;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Play drop sound if item collides with another object
        if (!string.IsNullOrEmpty(dropSoundName))
        {
            AudioManager.Instance.Play(dropSoundName);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 worldCenter = transform.position + transform.TransformDirection(checkBoxCenter);
        Gizmos.color = canDropItem ? Color.blue : Color.red;
        Gizmos.DrawWireCube(worldCenter, checkBoxSize);
    }
}
