'EOL'
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Volume Settings")]
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;

    [Header("Sound Effects")]
    public AudioClip popSound;
    public AudioClip swapSound;
    public AudioClip winSound;

    private AudioSource musicSource;
    private List<AudioSource> sfxPool = new List<AudioSource>();

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
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        
        for (int i = 0; i < 5; i++)
        {
            CreateSFXSource();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioSource source = GetAvailableSource();
        source.clip = clip;
        source.Play();
    }

    private AudioSource CreateSFXSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        sfxPool.Add(source);
        return source;
    }

    private AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in sfxPool)
        {
            if (!source.isPlaying) return source;
        }
        return CreateSFXSource();
    }
}
EOL
