using UnityEngine;

public class MoveableChair : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isMoved = false;

    public Transform targetPosition; // Target position and rotation
    public float moveDuration = 1f; // Duration of the move
    public AudioSource moveSoundSource; // Reference to the AudioSource component

    private Outline outlineComponent;
    private Collider chairCollider;

    private void Start()
    {
        // Save the original position and rotation of the chair
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Get the Outline component if it exists
        outlineComponent = GetComponent<Outline>();

        // Get the Collider component to disable interaction after moving
        chairCollider = GetComponent<Collider>();

        // Ensure the AudioSource component is assigned
        if (moveSoundSource == null)
        {
            Debug.LogError("AudioSource is not assigned in the MoveableChair script.");
        }
    }

    // Call this method to move the chair to the target position
    public void MoveChair()
    {
        if (!isMoved)
        {
            // Play the move sound if an AudioSource is provided
            if (moveSoundSource != null)
            {
                moveSoundSource.Play(); // Play the attached AudioSource
            }

            StopAllCoroutines(); // Stop any ongoing movement
            StartCoroutine(MoveToTarget(targetPosition.position, targetPosition.rotation));
            isMoved = true;
        }
    }

    private System.Collections.IEnumerator MoveToTarget(Vector3 targetPos, Quaternion targetRot)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime / moveDuration);
            transform.rotation = Quaternion.Slerp(startingRot, targetRot, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the chair ends at the exact target position and rotation
        transform.position = targetPos;
        transform.rotation = targetRot;

        // Disable the outline component after moving the chair
        if (outlineComponent != null)
        {
            outlineComponent.enabled = false;
        }

        // Disable the collider to prevent further interaction
        if (chairCollider != null)
        {
            chairCollider.enabled = false;
        }
    }
}
