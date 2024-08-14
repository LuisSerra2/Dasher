using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerData : PersistentSingleton<SoundManagerData>, IDataPersistence
{
    private AssetsManager assetsManager;

    private void Start()
    {
        assetsManager = AssetsManager.Instance;
    }

    public void SaveData(ref GameData data)
    {
        data.music = assetsManager.musicSlider.value;
        data.sound = assetsManager.soundsSlider.value;
    }
    public void LoadData(GameData data)
    {
        if (data == null) return;
        assetsManager.musicSlider.value = data.music;
        assetsManager.soundsSlider.value = data.sound;
    }
    public string GetUniqueIdentifier()
    {
        return this.gameObject.name + "_" + this.gameObject.GetInstanceID();
    }

}
