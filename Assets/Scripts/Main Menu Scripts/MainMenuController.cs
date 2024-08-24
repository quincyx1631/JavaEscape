using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels to Load")]
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Resolution DropDowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header("Progress Levels")]
    [SerializeField] private int defaultReachIndex = 1;
    [SerializeField] private int defaultUnlockedLevel = 1;
    [SerializeField] public GameObject levelSelector = null;
    [SerializeField] private GameObject newGameConfirmationDialog = null;

    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option  = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void CheckSavedGame()
    {
        int reachIndex = PlayerPrefs.GetInt("ReachIndex", defaultReachIndex);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", defaultUnlockedLevel);

        if (reachIndex == 1 && unlockedLevel == 1)
        {
            noSavedGameDialog.SetActive(true);
            levelSelector.SetActive(false);
            Debug.Log("No saved game found. Showing NoSavedGameDialog.");
        }
        else
        {
            noSavedGameDialog.SetActive(false);
            levelSelector.SetActive(true);
            Debug.Log("Saved game found. Showing LevelSelector.");
        }
    }

    public void newGameDialog()
    {
        int reachIndex = PlayerPrefs.GetInt("ReachIndex", defaultReachIndex);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", defaultUnlockedLevel);

        if (reachIndex > 1 && unlockedLevel > 1)
        {
            newGameConfirmationDialog.SetActive(true);
            levelSelector.SetActive(false);
            Debug.Log("No saved game found. Showing NoSavedGameDialog.");
        }
        else
        {
            newGameConfirmationDialog.SetActive(false);
            levelSelector.SetActive(true);
            Debug.Log("Saved game found. Showing LevelSelector.");
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
            //invert Y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
            //not invert
        }

        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
        StartCoroutine (ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool _isFullscreen)
    {
        _isFullScreen = _isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        //change your brightness with your post processsing or whatever it is

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0 ));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        int reachIndex = PlayerPrefs.GetInt("ReachIndex", defaultReachIndex);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", defaultUnlockedLevel);

        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if(MenuType == "Gameplay")
        {
            controllerSenTextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }

        if(MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = true;
            Screen.fullScreen = true;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if (MenuType == "Progress")
        {
            if (reachIndex > 1 && unlockedLevel > 1)
            {
                newGameConfirmationDialog.SetActive(true);
                levelSelector.SetActive(false);
                Debug.Log("Save Data Found. Showing New Game Confirmation.");
            }
            else if(reachIndex == 1 && unlockedLevel == 1)
            {
                newGameConfirmationDialog.SetActive(false);
                levelSelector.SetActive(true);
                Debug.Log("No Save Data Found. Showing LevelSelector.");

                //Reset Reach Index and Unlocked Levels
                PlayerPrefs.SetInt("ReachIndex", defaultReachIndex);
                PlayerPrefs.SetInt("UnlockedLevel", defaultUnlockedLevel);
                PlayerPrefs.Save();

                Debug.Log("Progress reset: ReachIndex and UnlockedLevel set to default values.");
            }
        }
        if(MenuType == "ResetOnly")
        {
            //Reset Reach Index and Unlocked Levels
            PlayerPrefs.SetInt("ReachIndex", defaultReachIndex);
            PlayerPrefs.SetInt("UnlockedLevel", defaultUnlockedLevel);
            PlayerPrefs.Save();

            Debug.Log("Progress reset: ReachIndex and UnlockedLevel set to default values.");
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
