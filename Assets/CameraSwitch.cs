using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;           // Reference to the main camera (could be player camera)
    public Camera secondaryCamera;      // Reference to the secondary camera (e.g., blackboard, map)
    public GameObject[] uiToDisable;    // Array of UI elements to disable when switching
    public GameObject[] uiToEnable;     // Array of UI elements to enable when switching
    public GameObject objectToHide;     // Object to hide (e.g., player, other elements)

    private bool isSecondaryCameraActive = false; // Track if the secondary camera view is active

    void Start()
    {
        // Ensure the main camera is active by default
        SwitchToMainCamera();
    }

    // Method to switch between cameras, to be called during interaction events
    public void SwitchCamera()
    {
        if (!isSecondaryCameraActive)
        {
            SwitchToSecondaryCamera();
        }
        else
        {
            SwitchToMainCamera();
        }
    }

    // Switch to the secondary camera, enable the specific UI, and hide object/UI
    void SwitchToSecondaryCamera()
    {
        secondaryCamera.enabled = true;
        mainCamera.enabled = false;

        // Hide the object if it exists
        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }

        // Disable the UI elements that should be hidden
        foreach (var uiElement in uiToDisable)
        {
            uiElement.SetActive(false);
        }

        // Enable the UI elements specific to the secondary camera (e.g., blackboard UI)
        foreach (var uiElement in uiToEnable)
        {
            uiElement.SetActive(true);
        }

        // Enable mouse control if needed
        MouseManager.Instance.EnableMouse();

        isSecondaryCameraActive = true;
    }

    // Switch back to the main camera, hide the specific UI, and show object/UI
    void SwitchToMainCamera()
    {
        secondaryCamera.enabled = false;
        mainCamera.enabled = true;

        // Show the object if it exists
        if (objectToHide != null)
        {
            objectToHide.SetActive(true);
        }

        // Enable the UI elements that should be shown
        foreach (var uiElement in uiToDisable)
        {
            uiElement.SetActive(true);
        }

        // Disable the UI elements specific to the secondary camera
        foreach (var uiElement in uiToEnable)
        {
            uiElement.SetActive(false);
        }

        // Disable mouse control
        MouseManager.Instance.DisableMouse();

        isSecondaryCameraActive = false;
    }

    void Update()
    {
        // Check for Escape key press and switch back to the main camera if needed
        if (Input.GetKeyDown(KeyCode.Escape) && isSecondaryCameraActive)
        {
            SwitchToMainCamera();
        }
    }
}
