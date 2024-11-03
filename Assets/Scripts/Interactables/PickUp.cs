using UnityEngine;
using TMPro;

public class PickUp : MonoBehaviour
{
    public Transform itemHolder;
    public string pickupSoundName;
    public string dropSoundName;
    public float dropForce = 5f;
    public float dropTorque = 10f;
    public Vector3 checkBoxSize = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 checkBoxCenter = Vector3.zero;
    public Collider playerCollider;
    public string itemLayerName = "Item";

    public Vector3 desiredRotation = Vector3.zero;
    public Vector3 positionOffset = Vector3.zero;

    public AlertUI alertUI;

    private Rigidbody rb;
    private Collider itemCollider;
    private bool canDropItem = true;
    private int originalLayer;
    private Vector3 originalWorldScale;
    private Transform originalParent;
    private bool hasCollided;
    private bool collisionDetectionEnabled = false;

    private bool itemOnHand = false;
    private bool isBeingPickedUp = false; // New flag to prevent immediate dropping

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.convex = true;
        }

        originalLayer = gameObject.layer;
        originalWorldScale = transform.lossyScale;
        originalParent = transform.parent;
    }

    private void Update()
    {
        bool isColliding = IsCollidingWithOtherObject();

        if (isColliding)
        {
            DisableDrop();
        }
        else
        {
            EnableDrop();
        }

        // Only process drop input if we're not in the process of picking up
        if (Input.GetKeyDown(KeyCode.G) && !isBeingPickedUp)
        {
            TryDropItem();
        }

        // Reset the pickup flag after a frame
        isBeingPickedUp = false;
    }

    public void PickUpItem()
    {
        if (itemHolder != null && itemHolder.childCount == 0 && !itemOnHand)
        {
            isBeingPickedUp = true; // Set the flag when picking up
            EnableDrop();
            if (!string.IsNullOrEmpty(pickupSoundName))
            {
                AudioManager.Instance.Play(pickupSoundName);
            }

            itemOnHand = true;

            transform.SetParent(null);
            originalWorldScale = transform.lossyScale;

            gameObject.layer = LayerMask.NameToLayer(itemLayerName);
            transform.SetParent(itemHolder);

            transform.localPosition = positionOffset;
            transform.localRotation = Quaternion.Euler(desiredRotation);

            ApplyWorldScale(originalWorldScale);

            if (itemCollider != null)
            {
                itemCollider.enabled = false;
            }
            rb.isKinematic = true;

            hasCollided = false;
            collisionDetectionEnabled = false;

            Invoke("EnableCollisionDetection", 0.1f);
        }
        else
        {
            Debug.Log("Cannot pick up item: Already holding another item.");
        }
    }

    // Rest of the code remains the same...

    public void Drop()
    {
        if (itemHolder != null && transform.parent == itemHolder && canDropItem && itemOnHand)
        {
            transform.SetParent(null);
            ApplyWorldScale(originalWorldScale);

            if (itemCollider != null)
            {
                itemCollider.enabled = true;
            }
            rb.isKinematic = false;

            if (Camera.main != null)
            {
                Vector3 forwardDirection = Camera.main.transform.forward;
                rb.AddForce(forwardDirection * dropForce + Vector3.up * 2f, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * dropTorque, ForceMode.Impulse);
            }

            gameObject.layer = originalLayer;

            collisionDetectionEnabled = true;
            hasCollided = false;

            itemOnHand = false;
        }
    }

    public void TryDropItem()
    {
        if (!canDropItem || !itemOnHand)
        {
            return;
        }

        Drop();
    }

    private void EnableCollisionDetection()
    {
        collisionDetectionEnabled = true;
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
