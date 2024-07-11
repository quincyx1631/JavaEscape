using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float PlayerReach = 3f;
    Interactables CurrentInteractables;
    public Camera PlayerCamera;
    public Transform itemHolder;  // Reference to the item holder

    void Update()
    {
        CheckInteraction();

        if (CurrentInteractables != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CurrentInteractables.Interact();
            }
            else if (Input.GetKeyDown(KeyCode.G) && IsItemHolderEmpty())
            {
                CurrentInteractables.PickUp();
            }
            else if (Input.GetKeyDown(KeyCode.E))
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

        // Debugging: Draw the ray in the Scene view
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

        // Only enable interaction text if item holder is empty
        if (IsItemHolderEmpty())
        {
            HUDController.instance.EnableInteractionText(CurrentInteractables.message);
        }
    }

    void DisableCurrentInteractables()
    {
        HUDController.instance.DisableInteractionText();
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
