using UnityEngine;
using System.Collections;

public class CorrectUIController : MonoBehaviour
{
    public static CorrectUIController Instance { get; private set; } // Singleton instance

    public GameObject correctUIInstance;  // Assign your Correct UI GameObject (in the Canvas) here
    private Animator animator;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Optional: Keeps it across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Get the Animator component from the assigned Correct UI instance
        if (correctUIInstance != null)
        {
            animator = correctUIInstance.GetComponent<Animator>();
            correctUIInstance.SetActive(false); // Make sure it's inactive at start
        }
        else
        {
            Debug.LogError("Correct UI instance not assigned in Inspector.");
        }
    }

    public void ShowCorrectUI()
    {
        if (correctUIInstance == null || animator == null)
        {
            Debug.LogError("Correct UI instance or Animator not set.");
            return;
        }

        // Activate the UI if it's not active
        if (!correctUIInstance.activeSelf)
        {
            correctUIInstance.SetActive(true);
        }

        // Start coroutine to play animation with a slight delay to avoid invalid layer index errors
        StartCoroutine(PlayCorrectUIAnimation());
    }

    private IEnumerator PlayCorrectUIAnimation()
    {
        // Wait for one frame to ensure that the animator is fully initialized
        yield return new WaitForEndOfFrame();

        // Play the animation on layer 0 (default layer)
        animator.Play("CheckAnimation", 0);

        // Optionally disable UI after the animation finishes
        StartCoroutine(DisableUIAfterAnimation());
    }

    private IEnumerator DisableUIAfterAnimation()
    {
        // Wait for the length of the animation
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // Disable the UI after the animation finishes
        correctUIInstance.SetActive(false);
        Debug.Log("UI deactivated after animation.");
    }
}
