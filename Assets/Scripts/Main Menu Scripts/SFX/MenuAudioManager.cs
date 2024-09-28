using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager Instance;

    public MainMenuSound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("BgMusic");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PlayMusic(string name)
    {
        MainMenuSound menuSounds = Array.Find(musicSounds, x => x.name == name);

        if (menuSounds == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            musicSource.clip = menuSounds.audioClip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        MainMenuSound menuSounds = Array.Find(sfxSounds, x => x.name == name);

        if (menuSounds == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            sfxSource.PlayOneShot(menuSounds.audioClip);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu Final")
        {
            PlayMusic("BgMusic");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
