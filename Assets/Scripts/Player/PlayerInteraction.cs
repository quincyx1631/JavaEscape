using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float PlayerReach = 3f;
    private Interactables CurrentInteractables;
    public Camera PlayerCamera;
    public Transform itemHolder;
    private PickUp currentItem;
    private bool raycastingEnabled = true;

    void Start()
    {
        if (PlayerCamera == null)
        {
            Debug.LogError("PlayerCamera is not assigned.");
        }
        if (itemHolder == null)
        {
            Debug.LogError("itemHolder is not assigned.");
        }
    }

    void Update()
    {
        if (raycastingEnabled)
        {
            CheckInteraction();

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
            UpdateUI();
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

    void UpdateUI()
    {
        if (CurrentInteractables == null)
        {
            Debug.LogWarning("CurrentInteractables is null. Cannot update UI.");
            return;
        }

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

    bool IsItemHolderEmpty()
    {
        return itemHolder != null && itemHolder.childCount == 0;
    }

    public void DisableRaycast()
    {
        raycastingEnabled = false;
        DisableCurrentInteractables();
    }

    public void EnableRaycast()
    {
        raycastingEnabled = true;

        // Check if we still have a valid CurrentInteractables when re-enabling raycast
        if (CurrentInteractables != null)
        {
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("CurrentInteractables is null after enabling raycast.");
        }
    }
}
