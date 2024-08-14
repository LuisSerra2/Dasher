using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsManager : PersistentSingleton<InputsManager>, IDataPersistence
{
    public CustomInput customInput = new CustomInput();

    public void SetKeyQ(KeyCode newKey)
    {
        customInput.keyQ = newKey;
        DataPersistenceManager.Instance.SaveGame();
    }

    public void SetKeyW(KeyCode newKey)
    {
        customInput.keyW = newKey;
        DataPersistenceManager.Instance.SaveGame();
    }

    public void SetKeyE(KeyCode newKey)
    {
        customInput.keyE = newKey;
        DataPersistenceManager.Instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        customInput.keyQ = data.ability1;
        customInput.keyW = data.ability2;
        customInput.keyE = data.ability3;
    }

    public void SaveData(ref GameData data)
    {

        data.ability1 = customInput.keyQ;
        data.ability2 = customInput.keyW;
        data.ability3 = customInput.keyE;
    }

    public string GetUniqueIdentifier()
    {
        return this.gameObject.name + "_" + this.gameObject.GetInstanceID();
    }
}

[Serializable]
public class CustomInput
{
    public KeyCode keyQ = KeyCode.Q;
    public KeyCode keyW = KeyCode.W;
    public KeyCode keyE = KeyCode.E;
}
