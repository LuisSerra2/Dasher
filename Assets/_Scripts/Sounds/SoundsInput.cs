using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsInput : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;

    [SerializeField] private Sprite musicToggleSpriteOn;
    [SerializeField] private Sprite musicToggleSpriteOff;
    [SerializeField] private Sprite soundToggleSpriteOn;
    [SerializeField] private Sprite soundToggleSpriteOff;

    private void Start()
    {
        SoundInputManager.Instance.Load();

        musicSlider.value = SoundInputManager.Instance.musicVolume;
        soundSlider.value = SoundInputManager.Instance.soundVolume;

        musicToggle.isOn = SoundInputManager.Instance.musicToggle;
        soundToggle.isOn = SoundInputManager.Instance.soundToggle;

        MusicToggle();
        SoundToggle();

        musicSlider.onValueChanged.AddListener(delegate { MusicSlider(); });
        soundSlider.onValueChanged.AddListener(delegate { SoundSlider(); });
        musicToggle.onValueChanged.AddListener(delegate { MusicToggle(); });
        soundToggle.onValueChanged.AddListener(delegate { SoundToggle(); });
    }

    private void MusicSlider()
    {
        SoundInputManager.Instance.SetMusicVolume(musicSlider.value);
        SoundManager.Instance.musicAudioSource.volume = musicSlider.value;
    }

    private void SoundSlider()
    {
        SoundInputManager.Instance.SetSoundVolume(soundSlider.value);
        SoundManager.Instance.soundEffectsAudioSource.volume = soundSlider.value;
    }

    private void MusicToggle()
    {
        SoundManager.Instance.musicAudioSource.mute = !musicToggle.isOn;
        SoundInputManager.Instance.SetMusicToggle(musicToggle.isOn);
        ChangeMusicSprite(musicToggle.isOn);
    }

    private void SoundToggle()
    {
        SoundManager.Instance.soundEffectsAudioSource.mute = !soundToggle.isOn;
        SoundInputManager.Instance.SetSoundToggle(soundToggle.isOn);
        ChangeSoundSprite(soundToggle.isOn);
    }

    public void ChangeMusicSprite(bool active)
    {
        musicToggle.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = active ? musicToggleSpriteOn : musicToggleSpriteOff;
    }
    public void ChangeSoundSprite(bool active)
    {
        soundToggle.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = active ? soundToggleSpriteOn : soundToggleSpriteOff;
    }
}
