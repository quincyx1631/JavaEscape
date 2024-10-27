using UnityEngine;
using TMPro; // If you're using TextMeshPro for the alert text

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

    public Vector3 desiredRotation = Vector3.zero; // Desired local rotation when item is picked up
    public Vector3 positionOffset = Vector3.zero;  // New position offset for when item is picked up

    public AlertUI alertUI; // Reference to the alert UI GameObject

    private Rigidbody rb;
    private Collider itemCollider;
    private bool canDropItem = true; // Flag to enable or disable dropping
    private int originalLayer; // Store the original layer
    private Vector3 originalWorldScale; // Store the original world scale
    private Transform originalParent; // Store the original parent
    private bool hasCollided; // Flag to check if the item has already collided
    private bool collisionDetectionEnabled = false; // Flag to delay collision detection

    // Item pickup status (non-static for instance-based tracking)
    private bool itemOnHand = false;

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

        // Store original properties
        originalLayer = gameObject.layer;
        originalWorldScale = transform.lossyScale; // Store the original world scale
        originalParent = transform.parent; // Store the original parent
    }

    private void Update()
    {
        // Check for collisions continuously and update the drop status
        bool isColliding = IsCollidingWithOtherObject();
        Debug.Log("Colliding: " + isColliding + ", canDropItem: " + canDropItem);

        if (isColliding)
        {
            DisableDrop();
        }
        else
        {
            EnableDrop();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            TryDropItem();
        }
    }

    public void PickUpItem()
    {
        Debug.Log("Attempting to pick up item: " + gameObject.name);

        if (itemHolder != null && itemHolder.childCount == 0 && !itemOnHand)
        {
            EnableDrop();
            Debug.Log("Picking up item: " + gameObject.name);

            if (!string.IsNullOrEmpty(pickupSoundName))
            {
                AudioManager.Instance.Play(pickupSoundName);
            }

            // Set itemOnHand to true to prevent picking up another item
            itemOnHand = true;

            // Detach from parent and store the original world scale
            transform.SetParent(null);
            originalWorldScale = transform.lossyScale;

            // Set the layer to "Item" before re-parenting
            gameObject.layer = LayerMask.NameToLayer(itemLayerName);

            // Re-parent to the item holder and apply the correct scale
            transform.SetParent(itemHolder);

            // Apply the position offset
            transform.localPosition = positionOffset;

            // Apply the specified desired rotation
            transform.localRotation = Quaternion.Euler(desiredRotation);

            ApplyWorldScale(originalWorldScale); // Apply the world scale

            // Disable the collider when picked up
            if (itemCollider != null)
            {
                itemCollider.enabled = false;
            }
            rb.isKinematic = true;

            // Reset collision flag
            hasCollided = false;
            collisionDetectionEnabled = false; // Disable collision detection initially
        }
        else
        {
            Debug.Log("Cannot pick up item: Already holding another item.");
        }
    }

    public void Drop()
    {
        if (itemHolder != null && transform.parent == itemHolder && canDropItem)
        {
            Debug.Log("Dropping item: " + gameObject.name);

            // Detach from parent and apply the original world scale
            transform.SetParent(null);
            ApplyWorldScale(originalWorldScale);

            // Re-enable the collider when dropped
            if (itemCollider != null)
            {
                itemCollider.enabled = true;
            }
            rb.isKinematic = false;

            // Add force and torque
            if (Camera.main != null)
            {
                Vector3 forwardDirection = Camera.main.transform.forward;
                rb.AddForce(forwardDirection * dropForce + Vector3.up * 2f, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * dropTorque, ForceMode.Impulse);
            }

            // Restore the original layer
            gameObject.layer = originalLayer;

            // Enable collision detection for the dropped item
            collisionDetectionEnabled = true;
            hasCollided = false;  // Reset the collision flag

            // Set itemOnHand to false to allow picking up another item
            itemOnHand = false;
        }
    }

    public void TryDropItem()
    {
        if (!canDropItem || !itemOnHand)
        {
            Debug.Log("Cannot drop item right now.");
            return;
        }

        Drop();
    }

    public bool IsCollidingWithOtherObject()
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
    }

    private void DisableDrop()
    {
        canDropItem = false;
    }

    private void ApplyWorldScale(Vector3 targetWorldScale)
    {
        transform.localScale = new Vector3(
            targetWorldScale.x / transform.lossyScale.x * transform.localScale.x,
            targetWorldScale.y / transform.lossyScale.y * transform.localScale.y,
            targetWorldScale.z / transform.lossyScale.z * transform.localScale.z
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided && collisionDetectionEnabled)
        {
            hasCollided = true;

            if (!string.IsNullOrEmpty(dropSoundName))
            {
                AudioManager.Instance.Play(dropSoundName);
            }

            collisionDetectionEnabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 worldCenter = transform.position + transform.TransformDirection(checkBoxCenter);
        Gizmos.color = canDropItem ? Color.blue : Color.red;
        Gizmos.DrawWireCube(worldCenter, checkBoxSize);
    }

    public void AlertChecker()
    {
        if (itemHolder.childCount >= 1 && !canDropItem)
        {
            alertUI.ShowAlert("The item shouldn't be colliding with other objects.");
        }
    }
}
