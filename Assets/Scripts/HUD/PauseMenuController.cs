using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PauseMenuController : MonoBehaviour
{
    private static PauseMenuController instance;
    public static PauseMenuController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PauseMenuController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PauseMenuController");
                    instance = obj.AddComponent<PauseMenuController>();
                }
            }
            return instance;
        }
    }

    [Header("Menu Objects")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Player Reference")]
    [SerializeField] private FirstPersonController firstPersonController;

    [Header("Settings")]
    [SerializeField] private bool canPauseWithTab = false;

    private bool isPaused = false;
    private float defaultVolume = 1f;
    private float defaultSensitivity = 2f;  // Changed to match FirstPersonController default

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize menu as hidden
        pauseMenuUI.SetActive(false);

        // Set default values
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", defaultVolume);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity);

        // Add listeners to sliders
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);

        // Add listeners to buttons
        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(LoadMainMenu);

        // Apply initial volume and sensitivity
        UpdateVolume(volumeSlider.value);
        UpdateSensitivity(sensitivitySlider.value);
    }

    public void canClickTab()
    {
        canPauseWithTab = true;
    }

    public void disableTab()
    {
        canPauseWithTab = false;
    }

    private void Update()
    {
        // Check for Tab key if canPauseWithTab is true
        if (canPauseWithTab && Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);

        // Disable camera movement
        if (firstPersonController != null)
            firstPersonController.cameraCanMove = false;

        // Show cursor for menu navigation
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        MouseManager.Instance.EnableMouse();
        PlayerControlManager.Instance.DisablePlayerControls();
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);

        // Re-enable camera movement
        if (firstPersonController != null)
            firstPersonController.cameraCanMove = true;

        // Hide cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MouseManager.Instance.DisableMouse();
        PlayerControlManager.Instance.EnablePlayerControls();
    }

    private void UpdateVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    private void UpdateSensitivity(float value)
    {
        if (firstPersonController != null)
            firstPersonController.mouseSensitivity = value;

        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }

    private void LoadMainMenu()
    {
        // Get all root objects in DontDestroyOnLoad
        GameObject[] rootObjects = GameObject.FindObjectsOfType<GameObject>(true)
            .Where(go => go.scene.buildIndex == -1 && go.transform.parent == null)
            .ToArray();

        // Destroy everything except PlayFab
        foreach (GameObject obj in rootObjects)
        {
            if (!obj.name.Contains("PlayFabHttp"))  // Adjust this condition based on your PlayFab object's name
            {
                Destroy(obj);
            }
        }

        // Reset pause state
        isPaused = false;

        disableTab();

        // Reset cursor state
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load main menu scene
        SceneManager.LoadScene("Main Menu Final");
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}