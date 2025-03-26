using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Volume Settings")]
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;
    public bool isMuted;

    [Header("Sound Effects")]
    public AudioClip popSound;
    public AudioClip swapSound;
    public AudioClip winSound;
    public AudioClip blastSound;

    [Header("Music")]
    public AudioClip mainTheme;
    public AudioClip gameOverMusic;

    private AudioSource musicSource;
    private List<AudioSource> sfxPool = new List<AudioSource>();
    private const int POOL_SIZE = 5;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Initialize()
    {
        // Music setup
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        // SFX pool
        for (int i = 0; i < POOL_SIZE; i++)
        {
            CreateSFXSource();
        }

        LoadVolumes();
        PlayMusic(mainTheme);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (isMuted || clip == null) return;
        musicSource.clip = clip;
        musicSource.volume = musicVolume * masterVolume;
        musicSource.Play();
    }

    public void PlayPop() => PlaySFX(popSound);
    public void PlaySwap() => PlaySFX(swapSound);
    public void PlayWin() => PlaySFX(winSound);
    public void PlayBlast() => PlaySFX(blastSound);

    public void PlaySFX(AudioClip clip)
    {
        if (isMuted || clip == null) return;

        AudioSource source = GetAvailableSource();
        source.clip = clip;
        source.volume = sfxVolume * masterVolume;
        source.Play();
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void ToggleMute(bool mute)
    {
        isMuted = mute;
        AudioListener.volume = mute ? 0 : 1;
    }

    private void UpdateVolumes()
    {
        musicSource.volume = isMuted ? 0 : musicVolume * masterVolume;
        
        foreach (var source in sfxPool)
        {
            source.volume = isMuted ? 0 : sfxVolume * masterVolume;
        }
    }

    private AudioSource GetAvailableSource()
    {
        foreach (var source in sfxPool)
        {
            if (!source.isPlaying) return source;
        }
        return CreateSFXSource();
    }

    private AudioSource CreateSFXSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        sfxPool.Add(source);
        return source;
    }

    private void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVol", 0.8f);
        musicVolume = PlayerPrefs.GetFloat("MusicVol", 0.7f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVol", 0.9f);
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        UpdateVolumes();
    }

    public void SaveVolumes()
    {
        PlayerPrefs.SetFloat("MasterVol", masterVolume);
        PlayerPrefs.SetFloat("MusicVol", musicVolume);
        PlayerPrefs.SetFloat("SFXVol", sfxVolume);
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}
