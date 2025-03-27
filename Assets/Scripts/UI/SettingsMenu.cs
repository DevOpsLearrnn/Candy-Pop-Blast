using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private Button defaultButton;
    [SerializeField] private AudioClip uiClickSound;

    private AudioSource uiAudioSource;

    void Start()
    {
        uiAudioSource = gameObject.AddComponent<AudioSource>();
        uiAudioSource.playOnAwake = false;

        LoadSettings();
        SetupEventListeners();
    }

    private void LoadSettings()
    {
        // Load saved values or use defaults
        masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 0.8f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.7f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 0.9f);
        muteToggle.isOn = PlayerPrefs.GetInt("Muted", 0) == 1;

        // Apply loaded settings
        ApplyCurrentSettings();
    }

    private void SetupEventListeners()
    {
        masterSlider.onValueChanged.AddListener(OnMasterChanged);
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        muteToggle.onValueChanged.AddListener(OnMuteChanged);
        defaultButton.onClick.AddListener(OnDefaultsClicked);
    }

    private void ApplyCurrentSettings()
    {
        AudioManager.Instance.SetMasterVolume(masterSlider.value);
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
        AudioManager.Instance.ToggleMute(muteToggle.isOn);
    }

    // ===== UI EVENT HANDLERS =====
    private void OnMasterChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
        PlayerPrefs.SetFloat("MasterVol", value);
        PlayClickSound();
    }

    private void OnMusicChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVol", value);
        PlayClickSound();
    }

    private void OnSFXChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVol", value);
        PlayClickSound();
    }

    private void OnMuteChanged(bool isMuted)
    {
        AudioManager.Instance.ToggleMute(isMuted);
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayClickSound();
    }

    private void OnDefaultsClicked()
    {
        // Reset to default values
        masterSlider.value = 0.8f;
        musicSlider.value = 0.7f;
        sfxSlider.value = 0.9f;
        muteToggle.isOn = false;

        // Save and apply
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
        PlayerPrefs.SetInt("Muted", 0);
        
        ApplyCurrentSettings();
        PlayClickSound();
    }

    private void PlayClickSound()
    {
        if (uiClickSound != null && !AudioManager.Instance.isMuted)
        {
            uiAudioSource.PlayOneShot(uiClickSound, 
                AudioManager.Instance.sfxVolume * AudioManager.Instance.masterVolume);
        }
    }

    void OnDestroy()
    {
        // Clean up listeners
        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        muteToggle.onValueChanged.RemoveAllListeners();
        defaultButton.onClick.RemoveAllListeners();

        // Ensure settings are saved
        PlayerPrefs.Save();
    }
}
