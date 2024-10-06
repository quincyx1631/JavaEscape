using UnityEngine;
using UnityEngine.Events;

public class Interactables : MonoBehaviour
{
    private Outline outline;
    private Inspect inspector;
    [SerializeField] private Transform itemHolder; // Assign in Inspector if possible

    public string message;

    public UnityEvent onInteraction;
    public UnityEvent onPickUp;
    public UnityEvent onInspect;

    public bool canInteract = true;
    public bool canPickup = true;
    public bool canInspect = true;

    public bool collectOnInteract = false; // Set true if collecting upon interaction
    public bool collectOnPickUp = false;   // Set true if collecting upon pickup
    public bool collectOnInspect = false;  // Set true if collecting upon inspection

    private bool isCollected = false;

    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();

        inspector = GetComponent<Inspect>(); // Get the Inspect component

        // If itemHolder isn't assigned via the Inspector, try to find it by name
        if (itemHolder == null)
        {
            itemHolder = GameObject.Find("ItemHolder")?.transform; // Find by name

            if (itemHolder == null)
            {
                Debug.LogError("ItemHolder not assigned and not found by name. Assign it in the Inspector.");
            }
        }
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void Interact()
    {
        if (canInteract)
        {
            onInteraction.Invoke();
            if (collectOnInteract)
            {
                Collect();
            }
        }
    }

    public void PickUp()
    {
        // Debugging to check what's happening during pickup
        if (itemHolder != null)
        {
            Debug.Log("ItemHolder child count: " + itemHolder.childCount);
        }

        // Ensure the player isn't already holding an item
        if (itemHolder != null && itemHolder.childCount == 0)
        {
            Debug.Log("Picking up item: " + gameObject.name);

            if (canPickup)
            {
                onPickUp.Invoke();
                if (collectOnPickUp)
                {
                    Collect();
                }
            }
        }
        else
        {
            Debug.Log("Cannot pick up item: Already holding another item.");
        }
    }

    public void Inspect()
    {
        if (canInspect)
        {
            onInspect.Invoke();
            if (collectOnInspect)
            {
                Collect();
            }
        }
    }

    public void StopInspecting()
    {
        inspector.StopInspection(); // Stop inspecting the item
    }

    private void Collect()
    {
        if (!isCollected)
        {
            isCollected = true;
            CollectionManager.Instance.MarkAsCollected(this);
            CorrectUIController.Instance.ShowCorrectUI();
        }
    }
}
