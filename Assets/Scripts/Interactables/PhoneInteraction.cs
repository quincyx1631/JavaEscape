using UnityEngine;

public class PhoneInteraction : MonoBehaviour
{
    public Transform phoneTargetPosition;  // Target position for the phone (in front of the camera)
    public GameObject uiElement;           // UI element to enable during interaction

    private Vector3 originalPosition;       // To store original position
    private Quaternion originalRotation;    // To store original rotation
    private Vector3 originalScale;          // To store original scale
    private bool isInteracting = false;

    private void Start()
    {
        // Store the original transform properties of the phone
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Detect escape key to end interaction
        if (isInteracting && Input.GetKeyDown(KeyCode.Escape))
        {
            EndInteraction();
        }
    }

    public void StartInteraction()
    {
        // Move the phone to the target position and rotation, keeping the original scale
        transform.position = phoneTargetPosition.position;
        transform.rotation = phoneTargetPosition.rotation;

        // Disable player movement
        PlayerControlManager.Instance.DisablePlayerControls();

        // Enable the UI element
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }

        isInteracting = true;
    }

    private void EndInteraction()
    {
        // Reset phone to its original position, rotation, and scale
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;

        // Enable player controls again
        PlayerControlManager.Instance.EnablePlayerControls();

        // Disable the UI element
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }

        isInteracting = false;
    }
}
