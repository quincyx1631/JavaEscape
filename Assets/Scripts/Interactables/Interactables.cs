using UnityEngine;
using UnityEngine.Events;

public class Interactables : MonoBehaviour
{
    private Outline outline;
    private Inspect inspector;
    private PickUp pickUpComponent;  // Reference to the PickUp component
    [SerializeField] private Transform itemHolder;
    public string message;
    public UnityEvent onInteraction;
    public UnityEvent onPickUp;
    public UnityEvent onInspect;
    public bool canInteract = true;
    public bool canPickup = true;
    public bool canInspect = true;
    public bool collectOnInteract = false;
    public bool collectOnPickUp = false;
    public bool collectOnInspect = false;
    private bool isCollected = false;

    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
        inspector = GetComponent<Inspect>();
        pickUpComponent = GetComponent<PickUp>();

        Debug.Log($"[Interactables] Initialized {gameObject.name}");
        Debug.Log($"[Interactables] Components - Outline: {outline != null}, Inspector: {inspector != null}, PickUp: {pickUpComponent != null}");

        if (itemHolder == null)
        {
            itemHolder = GameObject.Find("ItemHolder")?.transform;
            Debug.Log($"[Interactables] ItemHolder {(itemHolder != null ? "found" : "not found")} by name");
            if (itemHolder == null)
            {
                Debug.LogError("[Interactables] ItemHolder not assigned and not found by name. Assign it in the Inspector.");
            }
        }
    }

    public void DisableOutline()
    {
        if (outline != null) outline.enabled = false;
    }

    public void EnableOutline()
    {
        if (outline != null) outline.enabled = true;
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
        Debug.Log($"[Interactables] PickUp called for {gameObject.name}");
        Debug.Log($"[Interactables] State - CanPickup: {canPickup}, Has PickUp component: {pickUpComponent != null}");

        if (!canPickup)
        {
            Debug.Log($"[Interactables] {gameObject.name} cannot be picked up (canPickup is false)");
            return;
        }

        if (itemHolder != null && pickUpComponent != null)
        {
            Debug.Log($"[Interactables] Checking ItemHolder state - Child count: {itemHolder.childCount}");

            if (itemHolder.childCount == 0)
            {
                Debug.Log($"[Interactables] Beginning pickup sequence for {gameObject.name}");

                onPickUp.Invoke();
                Debug.Log($"[Interactables] onPickUp event invoked for {gameObject.name}");

                pickUpComponent.PickUpItem();

                if (collectOnPickUp)
                {
                    Debug.Log($"[Interactables] Collecting {gameObject.name}");
                    Collect();
                }
            }
            else
            {
                Debug.Log($"[Interactables] Cannot pick up {gameObject.name}: Already holding another item");
            }
        }
        else
        {
            Debug.LogWarning($"[Interactables] Missing required components for pickup on: {gameObject.name} - ItemHolder: {itemHolder != null}, PickUp component: {pickUpComponent != null}");
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
        if (inspector != null)
        {
            inspector.StopInspection();
        }
    }

    private void Collect()
    {
        if (!isCollected)
        {
            isCollected = true;
            if (CollectionManager.Instance != null)
            {
                CollectionManager.Instance.MarkAsCollected(this);
            }
            if (CorrectUIController.Instance != null)
            {
                CorrectUIController.Instance.ShowCorrectUI();
            }
        }
    }
}