using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private Outline outline;
    public Camera mainCamera;           // Reference to the main camera
    public Camera secondaryCamera;      // Reference to the secondary camera
    public GameObject[] uiToDisable;    // UI elements to disable when switching
    public GameObject[] uiToEnable;     // UI elements to enable when switching
    public GameObject objectToHide;     // Object to hide when switching
    public LineDrawer lineDrawer;       // Reference to the LineDrawer component

    [Header("Escape UI Settings")]
    public GameObject escapeUI;         // Escape UI GameObject
    public Animator escapeUIAnimator;   // Animator component for the escape UI animation

    [Header("Item Requirement")]
    public bool needObject = false;     // Set to true if the object is required for interaction
    public GameObject requiredItem;     // The item required to use the blackboard (e.g., Marker)
    public Transform itemHolder;        // The player's item holder where the required item should be
    public AlertUI alertUI;             // Reference to your existing AlertUI system for showing alerts

    private bool isSecondaryCameraActive = false; // Track if the secondary camera view is active

    void Start()
    {
        outline = GetComponent<Outline>();
        // Ensure the main camera is active by default
        SwitchToMainCamera();
    }

    public void SwitchCamera()
    {
        // If needObject is true, check if the player is holding the required item (e.g., Marker)
        if (needObject && !HasRequiredItem())
        {
            // Use your existing alertUI to display a message when the required item is missing
            if (alertUI != null)
            {
                alertUI.ShowAlert("You need a marker on hand to use this whiteboard.");
            }
            return; // Prevent switching camera if the item is not held
        }

        if (!isSecondaryCameraActive)
        {
            SwitchToSecondaryCamera();
        }
        else
        {
            SwitchToMainCamera();
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
                lineDrawer.ClearAllLines(); // Clear lines when switching back
                SwitchToMainCamera();
            }
        }
    }

    // Check if the player is holding the required item (marker) in the itemHolder
    private bool HasRequiredItem()
    {
        if (requiredItem != null && itemHolder != null)
        {
            // Check if the item in the itemHolder is the required item
            return itemHolder.childCount > 0 && itemHolder.GetChild(0).gameObject == requiredItem;
        }
        return false;
    }
}
