using UnityEngine;

public class WalkSoundManager : MonoBehaviour
{
    public string walkSoundName = "Walk"; // Name of the walking sound in the AudioManager
    private Rigidbody rb;                 // Reference to the player's Rigidbody
    private bool isWalking = false;       // Tracks if the walking sound is currently playing
    private float movementThreshold = 0.1f; // Minimum velocity to detect movement (can be adjusted)

    void Start()
    {
        // Get the Rigidbody component attached to the player
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("WalkSoundManager: No Rigidbody found on the player!");
        }
    }

    void Update()
    {
        // Ensure the Rigidbody is moving above the threshold
        if (rb != null && rb.velocity.magnitude > movementThreshold)
        {
            // If the player is moving and the sound isn't already playing, play the walking sound
            if (!isWalking)
            {
                isWalking = true;
                AudioManager.Instance.Play(walkSoundName);
            }
        }
        else
        {
            // If the player has stopped moving and the sound is playing, stop the walking sound
            if (isWalking)
            {
                isWalking = false;
                AudioManager.Instance.Stop(walkSoundName);
            }
        }
    }
}
