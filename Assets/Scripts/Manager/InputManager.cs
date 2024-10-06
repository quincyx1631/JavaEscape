using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private bool isInputBlocked = false; // Flag to check if input is blocked

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // If input is blocked, ignore all input events
        if (isInputBlocked) return;

        // Handle normal input processing here
        HandleInput();
    }

    public void BlockInput()
    {
        isInputBlocked = true; // Disable input
        Debug.Log("Input has been blocked");
    }

    public void UnblockInput()
    {
        isInputBlocked = false; // Enable input
        Debug.Log("Input has been unblocked");
    }

    private void HandleInput()
    {
        // Handle Escape key pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // This block will only run if input is NOT blocked
            if (!isInputBlocked)
            {
                Debug.Log("Escape Key Pressed");
                // Add your logic for what happens when Escape is pressed
            }
            return; // Exit the method after handling the Escape key
        }

        // Example input handling (replace with your actual input logic)
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Handle 'W' key pressed
            Debug.Log("W Key Pressed");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            // Handle 'A' key pressed
            Debug.Log("A Key Pressed");
        }

        // Add more input handling as needed
    }
}
