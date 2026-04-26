using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] public Slider volumeSlider;
    private float currentVolume = 1f; // Default volume level

    private void Start()
    {
        // Initialize the audio mixer with the default volume
        SetVolume(1f);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        currentVolume = volume;
        volumeSlider.value = volume;
        volume = Mathf.Clamp(volume, 0.0001f, 1f); // Ensure volume is within range
        volume = Mathf.Log10(volume) * 20f; // Convert to decibels, 0dB at 1.0, -80dB at 0.0001
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", currentVolume);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            SetVolume(PlayerPrefs.GetFloat("MasterVolume"));
        }
        else
        {
            SetVolume(1f); // Default volume if no saved setting exists
        }
    }
}
