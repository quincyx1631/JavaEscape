using UnityEngine;

public class ComputerCameraSwitch : MonoBehaviour
{
    private Outline outline; // Reference to the Outline component
    public Camera mainCamera;           // Reference to the main camera
    public Camera computerCamera;       // Reference to the computer camera
    public GameObject[] uiToDisable;    // UI elements to disable when switching
    public GameObject[] uiToEnable;     // UI elements to enable when switching
    public GameObject objectToHide;     // Object to hide when switching
    public GameObject escapeUI;         // Escape UI GameObject
    public Animator escapeUIAnimator;   // Animator component for the escape UI animation

    private bool isComputerCameraActive = false; // Track if the computer camera view is active
    private Computer currentComputer; // Reference to the current Computer instance

    void Start()
    {
        outline = GetComponent<Outline>();
        SwitchToMainCamera(); // Ensure the main camera is active by default
    }

    public void SwitchToComputerCamera(Computer computer)
    {
        currentComputer = computer; // Set the current computer reference

        if (!isComputerCameraActive)
        {
            SwitchToSecondaryCamera();
        }
        else
        {
            SwitchToMainCamera();
        }
    }

    private void ShowLoginUI()
    {
        // Check if the current computer exists and set the password
        if (currentComputer != null)
        {
            LoginUIManager.Instance.SetPassword(currentComputer.password);
        }
    }

    public void SwitchToSecondaryCamera()
    {
        outline.enabled = false;
        computerCamera.enabled = true;
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
        isComputerCameraActive = true;
        ShowLoginUI(); // Show the login UI when switching to the computer camera

        // Enable the escape UI
        if (escapeUI != null && escapeUIAnimator != null)
        {
            escapeUI.SetActive(true);
            escapeUIAnimator.SetBool("IsVisible", true);
        }
    }

    public void SwitchToMainCamera()
    {
        outline.enabled = true;
        computerCamera.enabled = false;
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
        isComputerCameraActive = false;

        // Disable the escape UI
        if (escapeUI != null && escapeUIAnimator != null)
        {
            escapeUIAnimator.SetBool("IsVisible", false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isComputerCameraActive)
        {
            SwitchToMainCamera();
        }
    }
}
