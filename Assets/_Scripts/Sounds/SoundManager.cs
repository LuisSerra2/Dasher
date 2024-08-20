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
public class SoundManager : MonoBehaviour, IDataPersistence
{
    public static SoundManager Instance;

    public SoundsData[] soundsDatas;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource soundEffectsAudioSource;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;

    private float music;
    private float sound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        MusicToggle();
        SoundToggle();
    }

    private void MusicSlider()
    {
        musicAudioSource.volume = musicSlider.value;
        music = musicSlider.value;
    }
    private void SoundSlider()
    {
        soundEffectsAudioSource.volume = soundSlider.value;
        sound = soundSlider.value;
    }

    private void MusicToggle()
    {
        if (!musicToggle.isOn)
        {
            SpritesController.SwitchSprite(Sprites.Music, SpritesSwitch.Second);
            musicAudioSource.volume = 0;
            music = musicSlider.value;
        } else
        {
            SpritesController.SwitchSprite(Sprites.Music, SpritesSwitch.Principal);
            MusicSlider();
        }

    }
    private void SoundToggle()
    {
        if (!soundToggle.isOn)
        {
            soundEffectsAudioSource.volume = 0;
            sound = soundSlider.value;
        } else
        {
            SoundSlider();
        }
    }

    public static void PlaySound(Enum sound, float volume)
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
                Instance.musicAudioSource.Stop();
                clip = Instance.soundsDatas[(int)musicType + Enum.GetValues(typeof(SoundType)).Length].Sounds;
                soundController = Instance.soundsDatas[(int)musicType + Enum.GetValues(typeof(SoundType)).Length].SoundController;
                Instance.musicAudioSource.clip = clip;
                Instance.musicAudioSource.loop = true;
                Instance.musicAudioSource.Play();
                return;

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
            return;
        } else if (soundController == SoundController.Sound)
        {
            Instance.soundEffectsAudioSource.PlayOneShot(clip, volume);
        }
    }

    public float ReturnMusicSlider() => musicSlider.value;
    public float ReturnSoundSlider() => soundSlider.value;

    public void Initialize()
    {
        musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        soundSlider = GameObject.Find("SoundsSlider").GetComponent<Slider>();
        musicToggle = GameObject.Find("MusicToggle").GetComponent<Toggle>();
        soundToggle = GameObject.Find("SoundToggle").GetComponent<Toggle>();

        musicSlider.onValueChanged.AddListener(delegate { MusicSlider(); });
        soundSlider.onValueChanged.AddListener(delegate { SoundSlider(); });

        MusicToggle();
        SoundToggle();

        musicSlider.value = music;
        soundSlider.value = sound;

        Debug.Log("asdasdasd");

    }

    public void LoadData(GameData data)
    {
        musicAudioSource.volume = data.music;
        soundEffectsAudioSource.volume = data.sound;
        musicToggle.isOn = data.musicON;
        soundToggle.isOn = data.soundON;
    }

    public void SaveData(ref GameData data)
    {
        data.music = musicAudioSource.volume;
        data.sound = soundEffectsAudioSource.volume;
        data.musicON = musicToggle.isOn;
        data.soundON = soundToggle.isOn;
    }

    public string GetUniqueIdentifier()
    {
        return this.gameObject.name + "_" + this.gameObject.GetInstanceID();
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
