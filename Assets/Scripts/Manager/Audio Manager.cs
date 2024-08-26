using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public bool loop;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;

        [HideInInspector]
        public AudioSource source;
        [HideInInspector]
        public Coroutine fadeOutCoroutine;
    }

    public List<Sound> sounds;

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
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (s.source == null)
        {
            Debug.LogError("AudioSource is null for sound: " + name);
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        Debug.Log($"Stopping sound: {name}");
        s.source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.UnPause();
    }

    public void FadeOutAndStop(string name, float fadeDuration)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.fadeOutCoroutine != null)
        {
            StopCoroutine(s.fadeOutCoroutine);
            s.fadeOutCoroutine = null;
        }

        s.fadeOutCoroutine = StartCoroutine(FadeOutCoroutine(s, fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(Sound sound, float fadeDuration)
    {
        float startVolume = sound.source.volume;
        Debug.Log($"Starting fade-out for {sound.name} over {fadeDuration} seconds");

        while (sound.source.volume > 0)
        {
            sound.source.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        sound.source.Stop();
        sound.source.volume = startVolume; // Reset volume for future playback
        sound.fadeOutCoroutine = null;
        Debug.Log($"{sound.name} stopped after fade-out");
    }

    public bool IsPlaying(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }
        return s.source.isPlaying;
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            if (s.source != null && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }
}
