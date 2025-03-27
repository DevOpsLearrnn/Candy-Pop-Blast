using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Volume Settings")]
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float musicVolume = 0.8f;
    [Range(0, 1)] public float sfxVolume = 0.9f;
    public bool isMuted;

    [Header("Music")]
    public AudioClip backgroundMusic; // Assign your game_theme.ogg here
    public AudioClip winMusic;
    public float musicFadeDuration = 1.5f;

    [Header("SFX")]
    public AudioClip popSound;
    public AudioClip swapSound;
    public AudioClip winSound;
    public AudioClip blastSound;

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
        musicSource.volume = musicVolume * masterVolume;

        // SFX pool
        for (int i = 0; i < POOL_SIZE; i++)
        {
            CreateSFXSource();
        }

        // Auto-play background music
        PlayMusic(backgroundMusic, fade: false);
    }

    // ===== MUSIC CONTROL =====
    public void PlayMusic(AudioClip clip, bool fade = true)
    {
        if (clip == null || isMuted) return;

        if (fade && musicSource.isPlaying)
        {
            StartCoroutine(FadeMusic(clip));
        }
        else
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float elapsed = 0f;
        float startVol = musicSource.volume;

        // Fade out current clip
        while (elapsed < musicFadeDuration)
        {
            musicSource.volume = Mathf.Lerp(startVol, 0, elapsed/musicFadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in new clip
        elapsed = 0f;
        while (elapsed < musicFadeDuration)
        {
            musicSource.volume = Mathf.Lerp(0, musicVolume * masterVolume, elapsed/musicFadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = musicVolume * masterVolume;
    }

    // ===== SFX CONTROL =====
    public void PlayPop() => PlaySFX(popSound);
    public void PlaySwap() => PlaySFX(swapSound);
    public void PlayWin() => PlaySFX(winSound);
    public void PlayBlast() => PlaySFX(blastSound);

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || isMuted) return;

        AudioSource source = GetAvailableSource();
        source.clip = clip;
        source.volume = sfxVolume * masterVolume;
        source.Play();
    }

    // ===== VOLUME CONTROL =====
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    public void ToggleMute(bool muted)
    {
        isMuted = muted;
        AudioListener.volume = muted ? 0 : 1;
    }

    private void UpdateAllVolumes()
    {
        musicSource.volume = isMuted ? 0 : musicVolume * masterVolume;
        
        foreach (var source in sfxPool)
        {
            if (source.isPlaying)
            {
                source.volume = isMuted ? 0 : sfxVolume * masterVolume;
            }
        }
    }

    // ===== POOL MANAGEMENT =====
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
}
