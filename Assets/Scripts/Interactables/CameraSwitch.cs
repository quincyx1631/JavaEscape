using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private Outline outline;
    public Camera mainCamera;           // Reference to the main camera
    public Camera secondaryCamera;      // Reference to the secondary camera
    public GameObject[] uiToDisable;    // UI elements to disable when switching
    public GameObject[] uiToEnable;     // UI elements to enable when switching
    public GameObject objectToHide;     // Object to hide when switching (e.g., Player)
    public LineDrawer lineDrawer;       // Optional reference to the LineDrawer component

    [Header("Escape UI Settings")]
    public GameObject escapeUI;         // Escape UI GameObject
    public Animator escapeUIAnimator;   // Animator component for the escape UI animation

    [Header("Item Requirement Settings")]
    public bool needItem = false;           // Flag to indicate if item is required to interact
    public CollectibleItem requiredItem;    // Reference to the required item
    public Transform itemPlaceholder;       // Optional ItemPlaceholder for placing the item
    public AlertUI alertUI;                 // Reference to your existing AlertUI system for showing alerts

    public Transform itemHolder;            // Transform to hold items

    [Header("Event Camera Settings")]
    public bool useEventCamera = true;     // Checkbox to enable or disable event camera switching

    private bool isSecondaryCameraActive = false; // Track if the secondary camera view is active

    void Start()
    {
        outline = GetComponent<Outline>();
        // Ensure the main camera is active by default
        SwitchToMainCamera();
    }

    public void SwitchCamera()
    {
        // Check if an item is required and if the player has collected it
        if (needItem && requiredItem != null && !requiredItem.IsCollected)
        {
            // Display an alert if the item is required but not collected
            if (alertUI != null)
            {
                alertUI.ShowAlert("You need to collect the required item first!");
            }
            return; // Prevent switching cameras
        }

        // If the item has been collected, check for the placeholder
        if (requiredItem != null && requiredItem.IsCollected && itemPlaceholder != null)
        {
            PlaceItemInPlaceholder();
        }

        // Switch between main and secondary cameras
        if (!isSecondaryCameraActive)
        {
            SwitchToSecondaryCamera();
        }
        else
        {
            SwitchToMainCamera();
        }
    }

    // Method to place the collected item into the item placeholder
    private void PlaceItemInPlaceholder()
    {
        if (requiredItem != null && itemPlaceholder != null)
        {
            requiredItem.transform.SetParent(itemPlaceholder);      // Set the ItemPlaceholder as the parent
            requiredItem.transform.localPosition = Vector3.zero;    // Position it correctly in the placeholder
            requiredItem.transform.localRotation = Quaternion.identity; // Reset rotation

            // Make the item static by disabling its Rigidbody physics
            Rigidbody itemRigidbody = requiredItem.GetComponent<Rigidbody>();
            if (itemRigidbody != null)
            {
                itemRigidbody.isKinematic = true; // Disable physics so the item doesn't fall
            }

            requiredItem.gameObject.SetActive(true); // Make the item visible in the placeholder

            // Change the tag of the item to "Untagged"
            requiredItem.tag = "Untagged";

            Debug.Log("Item placed in the placeholder, made static, and tag set to 'Untagged'.");
        }
    }

    public void SwitchToSecondaryCamera()
    {
        outline.enabled = false;

        // Enable secondary camera and disable main camera
        secondaryCamera.enabled = true;
        mainCamera.enabled = false;

        // Enable audio listener on secondary camera and disable on main camera
        secondaryCamera.GetComponent<AudioListener>().enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = false;

        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }

        // Drop interactable items from the ItemHolder
        DropInteractableItems();

        foreach (var uiElement in uiToDisable)
        {
            uiElement.SetActive(false);
        }

        foreach (var uiElement in uiToEnable)
        {
            uiElement.SetActive(true);
        }

        MouseManager.Instance.EnableMouse();
        isSecondaryCameraActive = true;

        // Set the event camera for all canvases in the scene if the option is enabled
        if (useEventCamera)
        {
            SetEventCamera(secondaryCamera);
        }

        // Enable the escape UI and set the "isVisible" boolean to true
        if (escapeUI != null && escapeUIAnimator != null)
        {
            escapeUI.SetActive(true); // Make sure the escape UI is active
            escapeUIAnimator.SetBool("IsVisible", true); // Set "isVisible" to true to show the UI
        }
    }

    public void SwitchToMainCamera()
    {
        outline.enabled = true;

        // Enable main camera and disable secondary camera
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;

        // Enable audio listener on main camera and disable on secondary camera
        mainCamera.GetComponent<AudioListener>().enabled = true;
        secondaryCamera.GetComponent<AudioListener>().enabled = false;

        if (objectToHide != null)
        {
            objectToHide.SetActive(true);
        }

        // Drop interactable items from the ItemHolder
        DropInteractableItems();

        foreach (var uiElement in uiToDisable)
        {
            uiElement.SetActive(true);
        }

        foreach (var uiElement in uiToEnable)
        {
            uiElement.SetActive(false);
        }

        MouseManager.Instance.DisableMouse();
        isSecondaryCameraActive = false;

        // Set the event camera for all canvases in the scene if the option is enabled
        if (useEventCamera)
        {
            SetEventCamera(mainCamera);
        }

        // Set the "isVisible" boolean to false to hide the escape UI
        if (escapeUI != null && escapeUIAnimator != null)
        {
            escapeUIAnimator.SetBool("IsVisible", false); // Set "isVisible" to false to hide the UI
        }
    }

    // Method to set the event camera for all canvases in the scene
    private void SetEventCamera(Camera camera)
    {
        // Find all canvases in the scene
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                canvas.worldCamera = camera; // Set the event camera for the canvas
                Debug.Log($"Setting canvas {canvas.name} event camera to: {camera.name}");
            }
        }
    }

    // Method to drop items with the tag "Interactables" from the ItemHolder
    private void DropInteractableItems()
    {
        if (itemHolder == null)
        {
            return; // Exit if no ItemHolder reference
        }

        foreach (Transform item in itemHolder) // Iterate through all items in the ItemHolder
        {
            if (item.CompareTag("Interactables"))
            {
                // Drop the item
                item.SetParent(null); // Remove the item from the ItemHolder
                item.position = itemHolder.position + Vector3.down * 1f; // Drop it slightly below the ItemHolder position

                // Enable the Rigidbody and Collider to make it fall
                Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
                if (itemRigidbody != null)
                {
                    itemRigidbody.isKinematic = false; // Make it fall
                }

                // Enable the Collider
                Collider itemCollider = item.GetComponent<Collider>();
                if (itemCollider != null)
                {
                    itemCollider.enabled = true; // Enable the collider
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSecondaryCameraActive)
            {
                // Check if lineDrawer is assigned before calling any methods
                if (lineDrawer != null)
                {
                    lineDrawer.ClearAllLines(); // Clear lines when switching back
                }
                SwitchToMainCamera();
            }
        }
    }
}
