using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float PlayerReach = 3f;
    private Interactables CurrentInteractables;
    public Camera PlayerCamera;
    public Transform itemHolder;
    private PickUp currentItem;

    void Update()
    {
        CheckInteraction();

        // Handle pickup interaction
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (CurrentInteractables != null && CurrentInteractables.canPickup && IsItemHolderEmpty())
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

        // Handle interaction and inspection
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
                if (CurrentInteractables != null && newInteractables != CurrentInteractables)
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
        if (CurrentInteractables != null)
        {
            CurrentInteractables.EnableOutline();

            if (IsItemHolderEmpty())
            {
                if (HUDController.instance != null)
                {
                    if (CurrentInteractables.canInteract)
                    {
                        HUDController.instance.EnableInteractImage();
                    }
                    else
                    {
                        HUDController.instance.DisableInteractImage();
                    }

                    if (CurrentInteractables.canPickup)
                    {
                        HUDController.instance.EnablePickupImage();
                    }
                    else
                    {
                        HUDController.instance.DisablePickupImage();
                    }

                    if (CurrentInteractables.canInspect)
                    {
                        HUDController.instance.EnableInspectImage();
                    }
                    else
                    {
                        HUDController.instance.DisableInspectImage();
                    }
                }
                else
                {
                    Debug.LogWarning("HUDController.instance is not assigned.");
                }
            }
        }
    }

    void DisableCurrentInteractables()
    {
        if (HUDController.instance != null)
        {
            HUDController.instance.DisableInteractImage();
            HUDController.instance.DisablePickupImage();
            HUDController.instance.DisableInspectImage();
        }
        else
        {
            Debug.LogWarning("HUDController.instance is not assigned.");
        }

        if (CurrentInteractables != null)
        {
            CurrentInteractables.DisableOutline();
            CurrentInteractables = null;
        }
    }

    bool IsItemHolderEmpty()
    {
        return itemHolder != null && itemHolder.childCount == 0;
    }
}
