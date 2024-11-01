using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of CanvasManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy the previous instance if one exists
        }
    }

    // You can add any methods related to your canvas functionality here
}
