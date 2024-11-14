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
    private float transitionDuration = 1.0f; // Duration for the transition effect
    private float transitionProgress = 0f;
    private bool isTransitioning = false;
    private Camera activeCamera;
    private Camera targetCamera;

    void Start()
    {
        outline = GetComponent<Outline>();
        // Ensure the main camera is active by default
        SwitchToMainCamera();
    }

    public void SwitchCamera()
    {
        if (itemHolder.childCount > 0)
        {
            if (alertUI != null)
            {
                alertUI.ShowAlert("You need to drop the item before switching cameras!");
            }
            return;
        }

        if (needItem && requiredItem != null && !requiredItem.IsCollected)
        {
            if (alertUI != null)
            {
                alertUI.ShowAlert("You need to collect the required item first!");
            }
            return;
        }

        if (requiredItem != null && requiredItem.IsCollected && itemPlaceholder != null)
        {
            PlaceItemInPlaceholder();
        }

        if (!isTransitioning)
        {
            if (!isSecondaryCameraActive)
            {
                StartTransition(mainCamera, secondaryCamera);
            }
            else
            {
                StartTransition(secondaryCamera, mainCamera);
            }
        }
    }

    private void StartTransition(Camera fromCamera, Camera toCamera)
    {
        activeCamera = fromCamera;
        targetCamera = toCamera;
        isTransitioning = true;
        transitionProgress = 0f;
        activeCamera.enabled = true;
        targetCamera.enabled = true;
    }

    private void PerformTransition()
    {
        PauseMenuController.Instance.disableTab();
        if (isTransitioning)
        {
            transitionProgress += Time.deltaTime / transitionDuration;
            float blend = Mathf.SmoothStep(0, 1, transitionProgress);

            activeCamera.fieldOfView = Mathf.Lerp(60, 40, blend);
            targetCamera.fieldOfView = Mathf.Lerp(40, 60, blend);

            if (transitionProgress >= 1f)
            {

                EndTransition();
            }
        }
    }

    private void EndTransition()
    {
        PauseMenuController.Instance.canClickTab();
        outline.enabled = false;    
        activeCamera.enabled = false;
        targetCamera.enabled = true;

        activeCamera.fieldOfView = 60;
        targetCamera.fieldOfView = 60;

        isSecondaryCameraActive = targetCamera == secondaryCamera;
        isTransitioning = false;

        if (isSecondaryCameraActive)
        {
            SwitchToSecondaryCamera();
        }
        else
        {
            SwitchToMainCamera();
        }
    }

    private void PlaceItemInPlaceholder()
    {
        if (requiredItem != null && itemPlaceholder != null)
        {
            requiredItem.transform.SetParent(itemPlaceholder);
            requiredItem.transform.localPosition = Vector3.zero;
            requiredItem.transform.localRotation = Quaternion.identity;

            Rigidbody itemRigidbody = requiredItem.GetComponent<Rigidbody>();
            if (itemRigidbody != null)
            {
                itemRigidbody.isKinematic = true;
            }

            requiredItem.gameObject.SetActive(true);
            requiredItem.tag = "Untagged";

            Debug.Log("Item placed in the placeholder, made static, and tag set to 'Untagged'.");
        }
    }

    public void SwitchToSecondaryCamera()
    {
        PauseMenuController.Instance.disableTab();

        outline.enabled = false;

        secondaryCamera.enabled = true;
        mainCamera.enabled = false;

        secondaryCamera.GetComponent<AudioListener>().enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = false;

        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }

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

        if (useEventCamera)
        {
            SetEventCamera(secondaryCamera);
        }

        if (escapeUI != null && escapeUIAnimator != null)
        {
            escapeUI.SetActive(true);
            escapeUIAnimator.SetBool("IsVisible", true);
        }
    }

    public void SwitchToMainCamera()
    {
        PauseMenuController.Instance.canClickTab();

        outline.enabled = true;

        mainCamera.enabled = true;
        secondaryCamera.enabled = false;

        mainCamera.GetComponent<AudioListener>().enabled = true;
        secondaryCamera.GetComponent<AudioListener>().enabled = false;

        if (objectToHide != null)
        {
            objectToHide.SetActive(true);
        }

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

        if (useEventCamera)
        {
            SetEventCamera(mainCamera);
        }

        if (escapeUI != null && escapeUIAnimator != null)
        {
            escapeUIAnimator.SetBool("IsVisible", false);
        }
    }

    private void SetEventCamera(Camera camera)
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                canvas.worldCamera = camera;
                Debug.Log($"Setting canvas {canvas.name} event camera to: {camera.name}");
            }
        }
    }

    private void DropInteractableItems()
    {
        if (itemHolder == null)
        {
            return;
        }

        foreach (Transform item in itemHolder)
        {
            if (item.CompareTag("Interactables"))
            {
                item.SetParent(null);
                item.position = itemHolder.position + Vector3.down * 1f;

                Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
                if (itemRigidbody != null)
                {
                    itemRigidbody.isKinematic = false;
                }

                Collider itemCollider = item.GetComponent<Collider>();
                if (itemCollider != null)
                {
                    itemCollider.enabled = true;
                }
            }
        }
    }

    void Update()
    {
        if (isTransitioning)
        {
            PerformTransition();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isSecondaryCameraActive)
        {
            if (lineDrawer != null)
            {
                lineDrawer.ClearAllLines();
            }
            SwitchToMainCamera();
        }
    }
}
