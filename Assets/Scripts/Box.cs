using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private Interactables interactables;

    void Start()
    {
        animator = GetComponent<Animator>();
        interactables = GetComponent<Interactables>();
        UpdateMessage();  // Update the initial message based on the initial state
    }

    public void Interact()
    {
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
        UpdateMessage();  // Update the message whenever the box is interacted with
        HUDController.instance.EnableInteractionText(interactables.message);  // Update the HUD with the new message
    }

    private void UpdateMessage()
    {
        if (isOpen)
        {
            interactables.message = "Close";
        }
        else
        {
            interactables.message = "Open";
        }
    }

    private void OnMouseEnter()
    {
        HUDController.instance.EnableInteractionText(interactables.message);
    }

    private void OnMouseExit()
    {
        HUDController.instance.DisableInteractionText();
    }
}
