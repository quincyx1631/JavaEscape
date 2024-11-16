using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MainMenuController menuController;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text musicTextValue = null;  
    [SerializeField] private Slider musicSlider = null;     
    [SerializeField] private TMP_Text sfxTextValue = null;   
    [SerializeField] private Slider sfxSlider = null;        


    [Header("Brightness Setting")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Header("Quality Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("FullScreen Setting")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    private void Awake()
    {
        if (!canUse) return;

        if (menuController == null)
        {
            Debug.LogError("MainMenuController reference not set in LoadPrefs!");
            return;
        }

        // Check for audio settings
        bool hasAudioSettings = PlayerPrefs.HasKey("masterVolume") &&
                               PlayerPrefs.HasKey("musicVolume") &&
                               PlayerPrefs.HasKey("sfxVolume");

        if (!hasAudioSettings)
        {
            try
            {
                menuController.ResetButton("Audio");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error resetting audio settings: {e.Message}");
            }
        }
        else
        {
            // Load existing audio settings...
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");
                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }

            if (PlayerPrefs.HasKey("musicVolume") && MenuAudioManager.Instance != null)
            {
                float localMusicVolume = PlayerPrefs.GetFloat("musicVolume");
                musicTextValue.text = localMusicVolume.ToString("0.0");
                musicSlider.value = localMusicVolume;
                MenuAudioManager.Instance.MusicVolume(localMusicVolume);
            }

            if (PlayerPrefs.HasKey("sfxVolume") && MenuAudioManager.Instance != null)
            {
                float localSfxVolume = PlayerPrefs.GetFloat("sfxVolume");
                sfxTextValue.text = localSfxVolume.ToString("0.0");
                sfxSlider.value = localSfxVolume;
                MenuAudioManager.Instance.SFXVolume(localSfxVolume);
            }

            if (!PlayerPrefs.HasKey("masterVolume") ||
                !PlayerPrefs.HasKey("musicVolume") ||
                !PlayerPrefs.HasKey("sfxVolume"))
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");
                Screen.fullScreen = localFullscreen == 1;
                fullScreenToggle.isOn = localFullscreen == 1;
            }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");
                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;
            }

            if (PlayerPrefs.HasKey("masterSen"))
            {
                float localSensitivity = PlayerPrefs.GetFloat("masterSen");
                controllerSenTextValue.text = localSensitivity.ToString("0.0");
                controllerSenSlider.value = localSensitivity;
                menuController.mainControllerSen = Mathf.RoundToInt(localSensitivity);
            }

            if (PlayerPrefs.HasKey("masterInvertY"))
            {
                invertYToggle.isOn = PlayerPrefs.GetInt("masterInvertY") == 1;
            }
        }
    }
}
