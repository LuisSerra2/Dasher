using System;
using UnityEngine;
using UnityEngine.UI;

public enum SoundController
{
    Music,
    Sound
}

public enum SoundType
{
    Bullet,
    Nuke,
    Spin,
    EnemyWarning,
    LaserWarning,
    Meteor,
    GameOver,
    ButtonsOnHover,
    ButtonsClick
}

public enum MusicType
{
    MainMenu,
    Playing,
    BossFight
}

[ExecuteInEditMode]
public class SoundManager : PersistentSingleton<SoundManager>
{
    public SoundsData[] soundsDatas;
    public AudioSource musicAudioSource;
    public AudioSource soundEffectsAudioSource;

    private void Start()
    {
        musicAudioSource.volume = SoundInputManager.Instance.musicVolume;
        soundEffectsAudioSource.volume = SoundInputManager.Instance.soundVolume;
    }

    public static void PlaySound(Enum sound)
    {
        AudioClip clip = null;
        SoundController soundController;

        switch (sound)
        {
            case SoundType soundType:
                clip = Instance.soundsDatas[(int)soundType].Sounds;
                soundController = Instance.soundsDatas[(int)soundType].SoundController;
                break;

            case MusicType musicType:
                clip = Instance.soundsDatas[(int)musicType + Enum.GetValues(typeof(SoundType)).Length].Sounds;
                soundController = Instance.soundsDatas[(int)musicType + Enum.GetValues(typeof(SoundType)).Length].SoundController;
                break;

            default:
                Debug.LogWarning("Unrecognized sound type: " + sound.ToString());
                return;
        }

        if (clip == null)
        {
            Debug.LogWarning("AudioClip is missing for " + sound.ToString());
            return;
        }

        if (soundController == SoundController.Music)
        {
            Instance.musicAudioSource.clip = clip;
            Instance.musicAudioSource.volume = SoundInputManager.Instance.musicVolume;
            Instance.musicAudioSource.Play();
        } else if (soundController == SoundController.Sound)
        {
            Instance.soundEffectsAudioSource.PlayOneShot(clip, SoundInputManager.Instance.soundVolume);
        }
    }

#if UNITY_EDITOR
    //private void OnEnable()
    //{
    //    InitializeSoundsDatas();
    //}

    //private void InitializeSoundsDatas()
    //{
    //    int totalSounds = Enum.GetValues(typeof(SoundType)).Length + Enum.GetValues(typeof(MusicType)).Length;
    //    soundsDatas = new SoundsData[totalSounds];

    //    int index = 0;

    //    int soundTypeCount = Enum.GetValues(typeof(SoundType)).Length;
    //    for (int i = 0; i < soundTypeCount; i++)
    //    {
    //        SoundType soundType = (SoundType)i;
    //        soundsDatas[index] = new SoundsData
    //        {
    //            Name = soundType.ToString(),
    //            SoundController = SoundController.Sound,
    //        };
    //        index++;
    //    }

    //    int musicTypeCount = Enum.GetValues(typeof(MusicType)).Length;
    //    for (int i = 0; i < musicTypeCount; i++)
    //    {
    //        MusicType musicType = (MusicType)i;
    //        soundsDatas[index] = new SoundsData
    //        {
    //            Name = musicType.ToString(),
    //            SoundController = SoundController.Music,
    //        };
    //        index++;
    //    }
    //}
#endif
}

[System.Serializable]
public struct SoundsData
{
    public string Name;
    public SoundController SoundController;
    public AudioClip Sounds;
}
