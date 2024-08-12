using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsController : Singleton<SoundsController>
{
    public AudioSource[] musicSource;
    public AudioSource[] soundsSource;

    public Slider musicSlider;
    public Slider soundsSlider;

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(delegate { ChangeMusicVolumeValue(); });
        soundsSlider.onValueChanged.AddListener(delegate { ChangeSoundsVolumeValue(); });

        UpdateAudioSourceVolumes();
    }

    private void ChangeMusicVolumeValue()
    {
        foreach (AudioSource music in musicSource)
        {
            music.volume = musicSlider.value;
        }
        DataPersistenceManager.Instance.SaveGame();
    }

    private void ChangeSoundsVolumeValue()
    {
        foreach (AudioSource sound in soundsSource)
        {
            sound.volume = soundsSlider.value;
        }
        DataPersistenceManager.Instance.SaveGame();
    }

    private void UpdateAudioSourceVolumes()
    {
        ChangeMusicVolumeValue();
        ChangeSoundsVolumeValue();
    }
}
