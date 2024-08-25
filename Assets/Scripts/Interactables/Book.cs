using ChristinaCreatesGames.Typography.Book;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private GameObject bookUI; // Reference to your book UI panel
    [SerializeField] private BookContents bookContents; // Reference to the BookContents script
    [TextArea(10, 20)][SerializeField] private string initialContent = "Default Book Content"; // Initial content for the book
    public FirstPersonController controller;
    private bool isUIActive = false; // Track if the UI is currently active
    private Collider itemCollider;
    private Rigidbody itemRigidbody;
    private Vector3 originalPosition; // Store original position before inspection
    private Quaternion originalRotation; // Store original rotation
    private Renderer bookRenderer; // Reference to the book's Renderer component
    public GameObject escapeUI;
    public string interactSoundName; // Name of the sound to play when interacting with the book

    void Start()
    {
        itemCollider = GetComponent<Collider>();
        itemRigidbody = GetComponent<Rigidbody>();
        bookRenderer = GetComponent<Renderer>(); // Get the Renderer component

        // Set initial content for the book
        if (bookContents != null)
        {
            bookContents.SetContent(initialContent);
        }
    }

    void Update()
    {
        if (isUIActive && Input.GetKeyDown(KeyCode.Escape))
        {
            HideBookUI();
        }
    }

    public void ShowBookUI()
    {
        HUDController.instance.EnableEscapeImage();
        if (bookUI != null)
        {
            Debug.Log("Showing Book UI");

            // Play the interaction sound
            if (!string.IsNullOrEmpty(interactSoundName))
            {
                AudioManager.Instance.Play(interactSoundName);
            }

            if (bookContents != null)
            {
                bookContents.SetContent(initialContent);
            }

            // Store original position and rotation
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            bookUI.SetActive(true);
            isUIActive = true;
            MouseManager.Instance.EnableMouse();
            PlayerControlManager.Instance.DisablePlayerControls();

            if (itemCollider != null)
            {
                itemCollider.enabled = false;
            }

            if (itemRigidbody != null)
            {
                itemRigidbody.isKinematic = true; // Prevent physics from affecting the book
            }

            if (bookRenderer != null)
            {
                bookRenderer.enabled = false; // Disable the Renderer to hide the book
            }
        }
    }

    public void HideBookUI()
    {
        if (bookUI != null)
        {
            Debug.Log("Hiding Book UI");

            // Hide the escape UI as well
            if (escapeUI != null)
            {
                HUDController.instance.DisableEscapeImage();
            }

            bookUI.SetActive(false);
            isUIActive = false;
            MouseManager.Instance.DisableMouse();
            PlayerControlManager.Instance.EnablePlayerControls();

            if (bookContents != null)
            {
                bookContents.ClearContent();
            }

            // Restore the book object's position and rotation
            transform.position = originalPosition;
            transform.rotation = originalRotation;

            if (bookRenderer != null)
            {
                bookRenderer.enabled = true; // Enable the Renderer to show the book
            }

            if (itemCollider != null)
            {
                itemCollider.enabled = true; // Re-enable the Collider for interaction
            }

            if (itemRigidbody != null)
            {
                itemRigidbody.isKinematic = false; // Re-enable physics for the book
            }
        }
    }


    public void SetBookContent(string content)
    {
        initialContent = content; // Update the initial content
        if (bookContents != null)
        {
            bookContents.SetContent(content);
        }
    }
}
