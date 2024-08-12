using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float PlayerReach = 3f;
    Interactables CurrentInteractables;
    public Camera PlayerCamera;
    public Transform itemHolder;
    private PickUp currentItem;

    void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentItem != null)
            {
                currentItem.Drop();
                Debug.Log("Item dropped: " + currentItem.gameObject.name);
                currentItem = null;
            }
            else if (CurrentInteractables != null && CurrentInteractables.canPickup && IsItemHolderEmpty())
            {
                CurrentInteractables.PickUp();
                currentItem = CurrentInteractables.GetComponent<PickUp>();
                if (currentItem != null)
                {
                    Debug.Log("Item picked up: " + currentItem.gameObject.name);
                }
                else
                {
                    Debug.LogWarning("Picked up item does not have a PickUp component.");
                }
            }
        }

        if (CurrentInteractables != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && CurrentInteractables.canInteract)
            {
                CurrentInteractables.Interact();
            }
            else if (Input.GetKeyDown(KeyCode.E) && CurrentInteractables.canInspect)
            {
                CurrentInteractables.Inspect();
            }
           
        }
    }

    void CheckInteraction()
    {
        if (PlayerCamera == null)
        {
            Debug.LogWarning("PlayerCamera is not assigned.");
            return;
        }

        RaycastHit hit;
        Ray ray = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * PlayerReach, Color.red);

        if (Physics.Raycast(ray, out hit, PlayerReach))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            if (hit.collider.CompareTag("Interactables"))
            {
                Interactables newInteractables = hit.collider.GetComponent<Interactables>();
                if (CurrentInteractables && newInteractables != CurrentInteractables)
                {
                    CurrentInteractables.DisableOutline();
                }

                if (newInteractables != null && newInteractables.enabled)
                {
                    SetNewCurrentInteractables(newInteractables);
                }
                else
                {
                    DisableCurrentInteractables();
                }
            }
            else
            {
                DisableCurrentInteractables();
            }
        }
        else
        {
            DisableCurrentInteractables();
        }
    }

    void SetNewCurrentInteractables(Interactables newInteractables)
    {
        CurrentInteractables = newInteractables;
        CurrentInteractables.EnableOutline();

        if (IsItemHolderEmpty())
        {
            if (CurrentInteractables.canInteract)
            {
                HUDController.instance.EnableInteractionText(CurrentInteractables.message);
            }
            else
            {
                HUDController.instance.DisableInteractionText();
            }

            if (CurrentInteractables.canPickup)
            {
                HUDController.instance.EnablePickupText();
            }
            else
            {
                HUDController.instance.DisablePickupText();
            }

            if (CurrentInteractables.canInspect)
            {
                HUDController.instance.EnableInspectText();
            }
            else
            {
                HUDController.instance.DisableInspectText();
            }
        }
    }

    void DisableCurrentInteractables()
    {
        HUDController.instance.DisableInteractionText();
        HUDController.instance.DisablePickupText();
        HUDController.instance.DisableInspectText();
        if (CurrentInteractables)
        {
            CurrentInteractables.DisableOutline();
            CurrentInteractables = null;
        }
    }

    bool IsItemHolderEmpty()
    {
        return itemHolder.childCount == 0;
    }
}
