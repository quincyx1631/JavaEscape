using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private Outline outline;
    public Camera mainCamera;           // Reference to the main camera
    public Camera secondaryCamera;      // Reference to the secondary camera
    public GameObject[] uiToDisable;    // UI elements to disable when switching
    public GameObject[] uiToEnable;     // UI elements to enable when switching
    public GameObject objectToHide;     // Object to hide when switching
    public LineDrawer lineDrawer;       // Optional reference to the LineDrawer component

    [Header("Escape UI Settings")]
    public GameObject escapeUI;         // Escape UI GameObject
    public Animator escapeUIAnimator;   // Animator component for the escape UI animation

    [Header("Item Requirement Settings")]
    public bool needItem = false;           // Flag to indicate if item is required to interact
    public CollectibleItem requiredItem;    // Reference to the required item
    public Transform itemPlaceholder;       // Optional ItemPlaceholder for placing the item
    public AlertUI alertUI;                 // Reference to your existing AlertUI system for showing alerts

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
        secondaryCamera.enabled = true;
        mainCamera.enabled = false;

        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }

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
        secondaryCamera.enabled = false;
        mainCamera.enabled = true;

        if (objectToHide != null)
        {
            objectToHide.SetActive(true);
        }

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

        // Set the "isVisible" boolean to false to hide the escape UI
        if (escapeUI != null && escapeUIAnimator != null)
        {
            escapeUIAnimator.SetBool("IsVisible", false); // Set "isVisible" to false to hide the UI
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
