using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Inspect : MonoBehaviour
{
    private bool isInspecting = false;
    private Quaternion originalRotation;
    private Quaternion initialRotation;
    private Vector3 originalPosition;
    private Vector3 lastMousePosition;
    private bool isRotating = false;

    public float rotationSpeed = 200f;
    public Camera inspectionCamera;
    public Transform inspectionPoint;
    private Collider itemCollider;
    private Rigidbody itemRigidbody;
    public GameObject escapeUI;
    public Light inspectionLight;
    public PostProcessVolume postProcessVolume;

    public string inspectStartSoundName;
    public string inspectStopSoundName;

    public PlayerInteraction playerInteraction;
    public Transform playerTransform;

    public Vector3 initialRotationEulerAngles;

    // Zoom variables
    public float zoomedFOV = 30f;  // Field of View when zoomed in
    public float zoomSpeed = 10f;  // Speed of FOV adjustment
    private float defaultFOV;      // Original FOV to revert back to after zooming

    void Start()
    {
        if (inspectionCamera != null)
        {
            defaultFOV = inspectionCamera.fieldOfView;
            inspectionCamera.enabled = false;
            inspectionCamera.gameObject.SetActive(false);
        }
        itemCollider = GetComponent<Collider>();
        itemRigidbody = GetComponent<Rigidbody>();
        if (inspectionLight != null)
        {
            inspectionLight.enabled = false;
        }

        if (postProcessVolume != null)
        {
            postProcessVolume.enabled = false;
        }
    }

    private void Update()
    {
        if (isInspecting)
        {
            HandleRotation();
            HandleZoom();

            transform.position = inspectionPoint.position;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopInspection();
            }
        }
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && isRotating)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            float angleX = mouseDelta.y * rotationSpeed * Time.deltaTime;
            float angleY = -mouseDelta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(inspectionCamera.transform.up, angleY, Space.World);
            transform.Rotate(inspectionCamera.transform.right, angleX, Space.World);

            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }
    }

    private void HandleZoom()
    {
        if (Input.GetMouseButton(1))  // Right-click held for zooming in
        {
            inspectionCamera.fieldOfView = Mathf.Lerp(inspectionCamera.fieldOfView, zoomedFOV, zoomSpeed * Time.deltaTime);
        }
        else
        {
            inspectionCamera.fieldOfView = Mathf.Lerp(inspectionCamera.fieldOfView, defaultFOV, zoomSpeed * Time.deltaTime);
        }
    }

    private Quaternion CalculateInitialRotation()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0;

        Quaternion playerRotation = Quaternion.LookRotation(-directionToPlayer, Vector3.up);
        return playerRotation * Quaternion.Euler(initialRotationEulerAngles);
    }

    public void StartInspection()
    {
        if (!isInspecting)
        {
            if (HUDController.instance != null)
            {
                HUDController.instance.EnableEscapeImage();
            }
            else
            {
                Debug.LogWarning("HUDController.instance is not assigned.");
            }

            isInspecting = true;
            originalRotation = transform.rotation;
            originalPosition = transform.position;

            initialRotation = CalculateInitialRotation();
            transform.rotation = initialRotation;

            if (!string.IsNullOrEmpty(inspectStartSoundName))
            {
                AudioManager.Instance.Play(inspectStartSoundName);
            }

            MouseManager.Instance.EnableMouse();
            PlayerControlManager.Instance.DisablePlayerControls();

            if (playerInteraction != null)
            {
                playerInteraction.DisableRaycast();
            }
            else
            {
                Debug.LogWarning("playerInteraction is not assigned.");
            }

            if (itemCollider != null) itemCollider.enabled = false;
            if (itemRigidbody != null) itemRigidbody.isKinematic = true;

            if (inspectionCamera != null)
            {
                inspectionCamera.enabled = true;
                inspectionCamera.gameObject.SetActive(true);
            }

            if (inspectionPoint != null)
            {
                inspectionPoint.gameObject.SetActive(true);
            }

            if (inspectionLight != null)
            {
                inspectionLight.enabled = true;
            }

            if (postProcessVolume != null)
            {
                postProcessVolume.enabled = true;
            }

            Time.timeScale = 0.5f;
        }
    }

    public void StopInspection()
    {
        if (isInspecting)
        {
            if (HUDController.instance != null)
            {
                HUDController.instance.DisableEscapeImage();
            }
            else
            {
                Debug.LogWarning("HUDController.instance is not assigned.");
            }

            isInspecting = false;

            if (!string.IsNullOrEmpty(inspectStopSoundName))
            {
                AudioManager.Instance.Play(inspectStopSoundName);
            }

            transform.position = originalPosition;
            transform.rotation = originalRotation;

            if (inspectionCamera != null)
            {
                inspectionCamera.enabled = false;
                inspectionCamera.gameObject.SetActive(false);
                inspectionCamera.fieldOfView = defaultFOV; // Reset FOV when inspection ends
            }

            if (inspectionPoint != null)
            {
                inspectionPoint.gameObject.SetActive(false);
            }

            if (inspectionLight != null)
            {
                inspectionLight.enabled = false;
            }

            if (postProcessVolume != null)
            {
                postProcessVolume.enabled = false;
            }

            Time.timeScale = 1f;

            if (itemCollider != null) itemCollider.enabled = true;
            if (itemRigidbody != null) itemRigidbody.isKinematic = false;

            MouseManager.Instance.DisableMouse();
            PlayerControlManager.Instance.EnablePlayerControls();

            if (playerInteraction != null)
            {
                playerInteraction.EnableRaycast();
            }
            else
            {
                Debug.LogWarning("playerInteraction is not assigned.");
            }
        }
    }
}
