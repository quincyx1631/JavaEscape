using UnityEngine;

public class ComputerCameraSwitch : MonoBehaviour
{
    private Outline outline; // Outline component to highlight interactable objects
    public Camera mainCamera; // Reference to the main camera
    public Camera computerCamera; // Reference to the computer's camera
    public GameObject[] uiToDisable; // UI elements to disable when switching
    public GameObject objectToHide; // Object to hide when switching (e.g., player's body)
    public GameObject escapeUI; // Escape UI for showing when in computer mode
    public Animator escapeUIAnimator; // Animator for escape UI

    private bool isComputerCameraActive = false; // Track if the computer camera is active
    private Computer currentComputer; // Current computer reference

    void Start()
    {
        outline = GetComponent<Outline>();
        SwitchToMainCamera(); // Start with the main camera active
    }

    public void SwitchToComputerCamera(Computer computer)
    {
        currentComputer = computer; // Set the current computer

        if (!isComputerCameraActive)
        {
            SwitchToSecondaryCamera();

            //FOR PAUSE DONT CHANGE -- NEIL
            PauseMenuController.Instance.disableTab();
        }
        else
        {
            SwitchToMainCamera();

            //FOR PAUSE DONT CHANGE -- NEIL
            PauseMenuController.Instance.canClickTab();
        }
    }

    private void ShowLoginOrDebugUI()
    {
        // If login is complete, show the Debug UI for the current computer
        if (currentComputer.isLoginComplete)
        {
            LoginUIManager.Instance.HideLoginUI();
            currentComputer.ToggleDebugUI(true); // Show the Debug UI specific to the computer
        }
        else
        {
            // Otherwise, show the Login UI and set up the password
            LoginUIManager.Instance.SetPassword(currentComputer.password);
            LoginUIManager.Instance.SetCurrentComputer(currentComputer);
            LoginUIManager.Instance.ShowLoginUI();
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

        MouseManager.Instance.EnableMouse();
        isComputerCameraActive = true;
        ShowLoginOrDebugUI(); // Show the appropriate UI based on the login state

        // Show the escape UI
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

        MouseManager.Instance.DisableMouse();
        isComputerCameraActive = false;

        // Hide the Login UI when switching back to the main camera
        LoginUIManager.Instance.HideLoginUI();

        // Hide the screen-space Debug UI when switching back to the main camera
        if (currentComputer != null && currentComputer.debugUI != null)
        {
            currentComputer.debugUI.SetActive(false); // Hide the screen-space Debug UI
        }

        // Check if the debug question has been answered
        if (currentComputer != null && currentComputer.isDebugAnswered)
        {
            // Only hide the World Space Debug UI if the debug has been answered
            if (currentComputer.worldSpaceDebugUI != null)
            {
                currentComputer.worldSpaceDebugUI.SetActive(false); // Hide the World Space Debug UI
            }
        }

        // Hide the escape UI
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
