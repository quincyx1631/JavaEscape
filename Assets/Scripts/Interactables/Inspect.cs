using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Inspect : MonoBehaviour
{
    private bool isInspecting = false;
    private Quaternion originalRotation;
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
    public PostProcessVolume postProcessVolume; // Reference to your Post-Processing Volume

    public string inspectStartSoundName; // Name of the sound to play when inspection starts
    public string inspectStopSoundName;  // Name of the sound to play when inspection stops

    void Start()
    {
        if (inspectionCamera != null)
        {
            inspectionCamera.enabled = false;
            inspectionCamera.gameObject.SetActive(false);
        }
        itemCollider = GetComponent<Collider>();
        itemRigidbody = GetComponent<Rigidbody>();
        if (inspectionLight != null)
        {
            inspectionLight.enabled = false;
        }

        // Make sure the PostProcessVolume is disabled at the start
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

    public void StartInspection()
    {
        if (!isInspecting)
        {
            escapeUI.SetActive(true);
            isInspecting = true;
            originalRotation = transform.rotation;
            originalPosition = transform.position;

            // Play the sound for starting inspection
            if (!string.IsNullOrEmpty(inspectStartSoundName))
            {
                AudioManager.Instance.Play(inspectStartSoundName);
            }

            MouseManager.Instance.EnableMouse();
            PlayerControlManager.Instance.DisablePlayerControls();

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
                postProcessVolume.enabled = true; // Enable the blur effect
            }

            Time.timeScale = 0.5f;
        }
    }

    public void StopInspection()
    {
        if (isInspecting)
        {
            escapeUI.SetActive(false);
            isInspecting = false;

            // Play the sound for stopping inspection
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
                postProcessVolume.enabled = false; // Disable the blur effect
            }

            if (itemCollider != null) itemCollider.enabled = true;
            if (itemRigidbody != null) itemRigidbody.isKinematic = false;

            Time.timeScale = 1f;
            PlayerControlManager.Instance.EnablePlayerControls();
            MouseManager.Instance.DisableMouse();
        }
    }
}
