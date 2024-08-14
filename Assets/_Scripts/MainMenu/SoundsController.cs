using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsController : MonoBehaviour
{
    private AssetsManager assetsManager;

    private void Start()
    {
        assetsManager = AssetsManager.Instance;

        assetsManager.musicSlider.onValueChanged.AddListener(delegate { ChangeMusicVolumeValue(); });
        assetsManager.soundsSlider.onValueChanged.AddListener(delegate { ChangeSoundsVolumeValue(); });

        UpdateAudioSourceVolumes();
        
    }

    private void ChangeMusicVolumeValue()
    {
        foreach (AudioSource music in assetsManager.musicSource)
        {
            music.volume = assetsManager.musicSlider.value;
        }

    }

    private void ChangeSoundsVolumeValue()
    {
        foreach (AudioSource sound in assetsManager.soundsSource)
        {
            sound.volume = assetsManager.soundsSlider.value;
        }
    }

    public void UpdateAudioSourceVolumes()
    {
        ChangeMusicVolumeValue();
        ChangeSoundsVolumeValue();
    }
}