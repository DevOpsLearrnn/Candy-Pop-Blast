using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Volume Settings")]
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;
    [Range(0, 1)] public float musicVolume = 1f;

    [Header("Sound Effects")]
    public AudioClip popSound;
    public AudioClip swapSound;
    public AudioClip winSound;
    public AudioClip blastSound;

    [Header("Background Music")]
    public AudioClip mainTheme;
    public AudioClip gameOverMusic;

    private AudioSource musicSource;
    private List<AudioSource> sfxPool = new List<AudioSource>();
    private int poolSize = 5;

    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern
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

        // Music source setup
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume * masterVolume;

        // SFX pool setup
        for (int i = 0; i < poolSize; i++)
        {
            GameObject sfxObj = new GameObject($"SFX_{i}");
            sfxObj.transform.SetParent(transform);
            AudioSource source = sfxObj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxPool.Add(source);
        }
    }

    void Start()
    {
        PlayMusic(mainTheme);
    }

    // === PUBLIC METHODS ===
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayPop() => PlaySFX(popSound);
    public void PlaySwap() => PlaySFX(swapSound);
    public void PlayWin() => PlaySFX(winSound);
    public void PlayBlast() => PlaySFX(blastSound);

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource availableSource = GetAvailableSource();
        if (availableSource != null)
        {
            availableSource.clip = clip;
            availableSource.volume = sfxVolume * masterVolume;
            availableSource.Play();
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume * masterVolume;
    }

    // === PRIVATE METHODS ===
    private AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in sfxPool)
        {
            if (!source.isPlaying) return source;
        }

        // If all sources are busy, expand pool
        return ExpandPool();
    }

    private AudioSource ExpandPool()
    {
        GameObject newObj = new GameObject($"SFX_{sfxPool.Count}");
        newObj.transform.SetParent(transform);
        AudioSource newSource = newObj.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        sfxPool.Add(newSource);
        return newSource;
    }

    private void UpdateAllVolumes()
    {
        musicSource.volume = musicVolume * masterVolume;
        
        foreach (AudioSource source in sfxPool)
        {
            if (source.isPlaying)
            {
                source.volume = sfxVolume * masterVolume;
            }
        }
    }
}
