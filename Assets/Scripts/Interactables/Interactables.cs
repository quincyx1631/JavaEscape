using UnityEngine;
using UnityEngine.Events;

public class Interactables : MonoBehaviour
{
    Outline outline;
    public string message;

    public UnityEvent onInteraction;
    public UnityEvent onPickUp;
    public UnityEvent onInspect;

    private Inspect inspector;

    // Start is called before the first frame update
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
        onInteraction.Invoke();
    }

    public void PickUp()
    {
        onPickUp.Invoke();
    }

    public void Inspect()
    {
        onInspect.Invoke();
       
    }

    public void StopInspecting()
    {
        inspector.StopInspection(); // Stop inspecting the item
    }
}
