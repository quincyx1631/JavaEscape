using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private FadeInIntro fadeInIntro;

    [Header("Player Reference")]
    [SerializeField] private FirstPersonController firstPersonController;

    private bool isPaused = false;
    private float defaultVolume = 1f;
    private float defaultSensitivity = 2f;  // Changed to match FirstPersonController default

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
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        // Apply initial volume and sensitivity
        UpdateVolume(volumeSlider.value);
        UpdateSensitivity(sensitivitySlider.value);
    }

    private void Update()
    {
        // Check for Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            Resume();
        else if (fadeInIntro == null || !fadeInIntro.isActiveAndEnabled)
            Pause();
    }

    public void Pause()
    {
        if (fadeInIntro != null && fadeInIntro.isActiveAndEnabled)
            return; // Don't pause if FadeInIntro is active

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

    public void ReturnToMainMenu()
    {
        // Show a confirmation dialog before returning to main menu
        ShowMainMenuConfirmation();
    }

    private void ShowMainMenuConfirmation()
    {
        // You can implement your own confirmation dialog UI here
        // For now, we'll just return to menu directly
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        // Reset pause state
        isPaused = false;

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