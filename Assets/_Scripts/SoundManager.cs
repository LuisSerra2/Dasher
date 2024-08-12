using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : PersistentSingleton<SoundManager>, IDataPersistence
{   
    public void SaveData(ref GameData data)
    {
        data.music = SoundsController.Instance.musicSlider.value;
        data.sound = SoundsController.Instance.soundsSlider.value;
    }
    public void LoadData(GameData data)
    {
        SoundsController.Instance.musicSlider.value = data.music;
        SoundsController.Instance.soundsSlider.value = data.sound;
    }
    public string GetUniqueIdentifier()
    {
        return this.gameObject.name + "_" + this.gameObject.GetInstanceID();
    }

}
