using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] GameObject mainMenuButton;
    [SerializeField] Canvas audioCanvas;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MainVolume", 1);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
    }

    public void SetVolumeLevel(float sliderValue)
    {
        audioMixer.SetFloat("MainVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MainVolume", sliderValue);
    }

    public void IncreaseVolumeLevel()
    {
        SetVolumeLevel(masterVolumeSlider.value + 10f);
    }

    public void DecreaseVolumeLevel()
    {
        SetVolumeLevel(masterVolumeSlider.value - 10f);
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void IncreaseMusicVolumeLevel()
    {
        SetVolumeLevel(musicVolumeSlider.value + 0.1f);
    }

    public void DecreaseMusicVolumeLevel()
    {
        SetVolumeLevel(musicVolumeSlider.value - 0.1f);
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    public void IncreaseSFXVolumeLevel()
    {
        SetVolumeLevel(sfxVolumeSlider.value + 0.1f);
    }

    public void DecreaseSFXVolumeLevel()
    {
        SetVolumeLevel(sfxVolumeSlider.value - 0.1f);
    }
}
