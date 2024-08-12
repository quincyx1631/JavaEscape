using UnityEngine;
using TMPro;
using ChristinaCreatesGames.Typography.Book;

public class Book : MonoBehaviour
{
    [SerializeField] private GameObject bookUI; // Reference to your book UI panel
    [SerializeField] private BookContents bookContents; // Reference to the BookContents script
    [TextArea(10, 20)][SerializeField] private string initialContent = "Default Book Content"; // Initial content for the book
    public FirstPersonController controller;
    private bool isUIActive = false; // Track if the UI is currently active
    private Collider itemCollider;
    private Vector3 originalPosition; // Store original position before inspection
    private Quaternion originalRotation; // Store original rotation
    private Renderer bookRenderer; // Reference to the book's Renderer component
    public GameObject escapeUI;
    void Start()
    {
        itemCollider = GetComponent<Collider>();
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
            Debug.Log("Hiding Book UI");
            HideBookUI();
        }
    }

    public void ShowBookUI()
    {
        escapeUI.SetActive(true);
        if (bookUI != null)
        {
            Debug.Log("Showing Book UI");
            if (bookContents != null)
            {
                bookContents.SetContent(initialContent);
            }
            bookUI.SetActive(true);
            isUIActive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerControlManager.Instance.DisablePlayerControls();
            itemCollider.enabled = false;

            // Hide the book object
            if (bookRenderer != null)
            {
                bookRenderer.enabled = false; // Disable the Renderer to hide the book
            }

            originalPosition = transform.position; // Store original position
            originalRotation = transform.rotation; // Store original rotation
        }
    }

    public void HideBookUI()
    {
        escapeUI.SetActive(false);
        if (bookUI != null)
        {
            Debug.Log("Hiding Book UI");
            bookUI.SetActive(false);
            isUIActive = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerControlManager.Instance.EnablePlayerControls();

            if (bookContents != null)
            {
                bookContents.ClearContent();
            }

            // Show the book object
            if (bookRenderer != null)
            {
                bookRenderer.enabled = true; // Enable the Renderer to show the book
            }
        }
        itemCollider.enabled = true;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
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
