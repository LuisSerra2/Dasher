using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssetsManager : PersistentSingleton<AssetsManager>
{
    public AudioSource[] musicSource;
    public AudioSource[] soundsSource;

    public Slider musicSlider;
    public Slider soundsSlider;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    private void Initialize()
    {
        musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        soundsSlider = GameObject.Find("SoundsSlider").GetComponent<Slider>();
    }
}
