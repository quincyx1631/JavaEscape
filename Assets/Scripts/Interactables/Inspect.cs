using UnityEngine;

public class Inspect : MonoBehaviour
{
    private bool isInspecting = false;
    private Vector3 inspectRotation;
    private Vector3 originalPosition; // Store original position before inspection
    private Quaternion originalRotation; // Store original rotation

    // Adjust the sensitivity as needed
    public float rotationSpeed = 5f;

    // Reference to the inspection camera
    public Camera inspectionCamera;
    public Transform inspectionPoint;
    public FirstPersonController controller;
    private Collider itemCollider;
    // Cube or point where the item should be placed

    // Start is called before the first frame update
    void Start()
    {
        // Ensure inspection camera is disabled initially
        if (inspectionCamera != null)
        {
            inspectionCamera.enabled = false;
            inspectionCamera.gameObject.SetActive(false);
        }
        itemCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isInspecting)
        {
            // Rotate the item based on mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            inspectRotation.y += mouseX * rotationSpeed;
            inspectRotation.x -= mouseY * rotationSpeed;

            transform.rotation = Quaternion.Euler(inspectRotation);

            // Position the item at the inspection point
            if (inspectionPoint != null)
            {
                transform.position = inspectionPoint.position;
            }
        }

        // Check for escape key to stop inspection
        if (isInspecting && Input.GetKeyDown(KeyCode.Escape))
        {
            StopInspection();
        }
    }

    public void StartInspection()
    {
        if (!isInspecting)
        {
            isInspecting = true;
            inspectRotation = transform.eulerAngles;
            originalPosition = transform.position; // Store original position
            originalRotation = transform.rotation; // Store original rotation

            // Disable player controls (if applicable)
            DisablePlayerControls();

            itemCollider.enabled = false;
            // Enable the inspection camera
            if (inspectionCamera != null)
            {
                inspectionCamera.enabled = true;
                inspectionCamera.gameObject.SetActive(true);
            }

            // Optionally, activate the inspection point (if needed)
            if (inspectionPoint != null)
            {
                inspectionPoint.gameObject.SetActive(true);
            }

            // Position the item at the inspection point
            if (inspectionPoint != null)
            {
                transform.position = inspectionPoint.position;
            }

            // Optionally, freeze time or adjust game speed for inspection mode
            Time.timeScale = 0.5f; // Example: Slow down time for inspection (adjust as needed)
        }
    }

    public void StopInspection()
    {
        if (isInspecting)
        {
            isInspecting = false;
            // Reset position and rotation to original
            transform.position = originalPosition;
            transform.rotation = originalRotation;

            // Disable the inspection camera
            if (inspectionCamera != null)
            {
                inspectionCamera.enabled = false;
                inspectionCamera.gameObject.SetActive(false);
            }

            // Optionally, deactivate the inspection point (if activated)
            if (inspectionPoint != null)
            {
                inspectionPoint.gameObject.SetActive(false);
            }

            // Enable player controls (if applicable)
            EnablePlayerControls();
            itemCollider.enabled = true;

            // Optionally, reset time scale or game speed adjustments
            Time.timeScale = 1f; // Reset time scale to normal
            
        }
    }

    private void DisablePlayerControls()
    {
        // Example: Disable player movement (adjust according to your player control setup)
      controller.playerCanMove = false;
        controller.cameraCanMove = false;
   

        // Example: Disable other scripts responsible for player movement
        // PlayerMovementScript.enabled = false;
    }

    private void EnablePlayerControls()
    {
        // Example: Enable player movement (adjust according to your player control setup)

       controller.playerCanMove = true;
        controller.cameraCanMove = true;
        // Example: Enable other scripts responsible for player movement
        // PlayerMovementScript.enabled = true;
    }
}
