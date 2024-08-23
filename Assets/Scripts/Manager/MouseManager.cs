using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance { get; private set; }

    public GameObject crosshair; // Reference to the crosshair GameObject

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of MouseManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps this instance persistent across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Enables the mouse cursor and shows the crosshair.
    /// </summary>
    public void EnableMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }

    /// <summary>
    /// Disables the mouse cursor and hides the crosshair.
    /// </summary>
    public void DisableMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
    }
}
