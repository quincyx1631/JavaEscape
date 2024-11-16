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

    [SerializeField] private TMP_Text musicTextValue = null; 
    [SerializeField] private Slider _musicSlider = null; 
    [SerializeField] private float defaultMusicVolume = 1.0f; 

    [SerializeField] private TMP_Text sfxTextValue = null; 
    [SerializeField] private Slider _sfxSlider = null; 
    [SerializeField] private float defaultSfxVolume = 1.0f; 

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

    [Header("Resolution DropDowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

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

        PlayerPrefs.SetFloat("musicVolume", _musicSlider.value);

        PlayerPrefs.SetFloat("sfxVolume", _sfxSlider.value);

        StartCoroutine(ConfirmationBox());
    }

    public void SetMusicVolume(float musicVolume)
    {
        if (MenuAudioManager.Instance != null)  // Add null check
        {
            MenuAudioManager.Instance.MusicVolume(musicVolume);
            if (musicTextValue != null)  // Add null check for UI
            {
                musicTextValue.text = musicVolume.ToString("0.0");
            }
        }
    }

    public void SetSFXVolume(float sfxVolume)
    {
        if (MenuAudioManager.Instance != null)  // Add null check
        {
            MenuAudioManager.Instance.SFXVolume(sfxVolume);
            if (sfxTextValue != null)  // Add null check for UI
            {
                sfxTextValue.text = sfxVolume.ToString("0.0");
            }
        }
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
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
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
        if (MenuType == "Audio")
        {
            // Add null checks for each component
            if (volumeSlider != null && volumeTextValue != null)
            {
                AudioListener.volume = defaultVolume;
                volumeSlider.value = defaultVolume;
                volumeTextValue.text = defaultVolume.ToString("0.0");
            }

            if (_musicSlider != null && musicTextValue != null && MenuAudioManager.Instance != null)
            {
                _musicSlider.value = defaultMusicVolume;
                MenuAudioManager.Instance.MusicVolume(defaultMusicVolume);
                musicTextValue.text = defaultMusicVolume.ToString("0.0");
            }

            if (_sfxSlider != null && sfxTextValue != null && MenuAudioManager.Instance != null)
            {
                _sfxSlider.value = defaultSfxVolume;
                MenuAudioManager.Instance.SFXVolume(defaultSfxVolume);
                sfxTextValue.text = defaultSfxVolume.ToString("0.0");
            }

            VolumeApply();
        }

        if (MenuType == "Gameplay")
        {
            if (controllerSenTextValue != null && controllerSenSlider != null)
            {
                controllerSenTextValue.text = defaultSen.ToString("0");
                controllerSenSlider.value = defaultSen;
                mainControllerSen = defaultSen;
            }

            if (invertYToggle != null)
            {
                invertYToggle.isOn = false;
            }

            GameplayApply();
        }

        if (MenuType == "Graphics")
        {
            if (brightnessSlider != null && brightnessTextValue != null)
            {
                brightnessSlider.value = defaultBrightness;
                brightnessTextValue.text = defaultBrightness.ToString("0.0");
            }

            if (qualityDropdown != null)
            {
                qualityDropdown.value = 1;
                QualitySettings.SetQualityLevel(1);
            }

            if (fullScreenToggle != null)
            {
                fullScreenToggle.isOn = true;
                Screen.fullScreen = true;
            }

            if (resolutionDropdown != null)
            {
                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
                resolutionDropdown.value = resolutions.Length;
            }

            GraphicsApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    public void MusicVolume()
    {
        MenuAudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        MenuAudioManager.Instance.SFXVolume(_sfxSlider.value);
    }
}
