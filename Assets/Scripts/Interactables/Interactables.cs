using UnityEngine;
using UnityEngine.Events;

public class Interactables : MonoBehaviour
{
    private Outline outline;
    private Inspect inspector;

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
        if (canPickup)
        {
            onPickUp.Invoke();
            if (collectOnPickUp)
            {
                Collect();
            }
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
        }
    }
}
