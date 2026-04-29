using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource[] soundEffectSources;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        soundVolumeSlider.onValueChanged.AddListener(UpdateSoundVolume);
        UpdateMusicVolume(musicVolumeSlider.value);
        UpdateSoundVolume(soundVolumeSlider.value);
    }

    private AudioSource GetAvailableSoundSource()
    {
        foreach (var source in soundEffectSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }

    private void UpdateMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    private void UpdateSoundVolume(float volume)
    {
        foreach (var source in soundEffectSources)
        {
            source.volume = volume;
        }
    }
}
