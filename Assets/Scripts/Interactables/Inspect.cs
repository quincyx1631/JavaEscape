using UnityEngine;
using UnityEngine.UI;

public class Inspect : MonoBehaviour
{
    private bool isInspecting = false;
    private Vector3 inspectRotation;
    private Vector3 originalPosition; // Store original position before inspection
    private Quaternion originalRotation; // Store original rotation

    public float rotationSpeed = 5f;
    public float zoomSpeed = 0.1f;
    public float minZoom = 0.5f;
    public float maxZoom = 3f;
    public float minRotationX = -60f;
    public float maxRotationX = 60f;

    public Camera inspectionCamera;
    public Transform inspectionPoint;
    private Collider itemCollider;
    public GameObject escapeUI;
    public Light inspectionLight;

    private float currentZoom = 1f;

    void Start()
    {
        if (inspectionCamera != null)
        {
            inspectionCamera.enabled = false;
            inspectionCamera.gameObject.SetActive(false);
        }
        itemCollider = GetComponent<Collider>();
        if (inspectionLight != null)
        {
            inspectionLight.enabled = false;
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
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        inspectRotation.y += mouseX * rotationSpeed;
        inspectRotation.x = Mathf.Clamp(inspectRotation.x - mouseY * rotationSpeed, minRotationX, maxRotationX);

        transform.rotation = Quaternion.Euler(inspectRotation);
    }

    private void HandleZoom()
    {
        if (Input.GetMouseButton(0)) // Left mouse button for zooming
        {
            float mouseY = Input.GetAxis("Mouse Y");
            currentZoom = Mathf.Clamp(currentZoom - mouseY * zoomSpeed, minZoom, maxZoom);

            // Calculate new position based on zoom level
            Vector3 direction = (transform.position - inspectionPoint.position).normalized;
            transform.position = inspectionPoint.position + direction * currentZoom;
        }
    }

    public void StartInspection()
    {
        if (!isInspecting)
        {
            escapeUI.SetActive(true);
            isInspecting = true;
            inspectRotation = transform.eulerAngles;
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            PlayerControlManager.Instance.DisablePlayerControls();
            itemCollider.enabled = false;

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

            Time.timeScale = 0.5f;
        }
    }

    public void StopInspection()
    {
        if (isInspecting)
        {
            escapeUI.SetActive(false);
            isInspecting = false;

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

            
            itemCollider.enabled = true;

            Time.timeScale = 1f;
            PlayerControlManager.Instance.EnablePlayerControls();
        }
    }

  
}
