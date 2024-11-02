using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsManager : PersistentSingleton<InputsManager>
{

    public CustomInput customInput = new CustomInput();

    public void SetKeyQ(KeyCode newKey)
    {
        customInput.keyQ = newKey;
    }

    public void SetKeyW(KeyCode newKey)
    {
        customInput.keyW = newKey;
    }

    public void SetKeyE(KeyCode newKey)
    {
        customInput.keyE = newKey;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("KeyQ", (int)customInput.keyQ);
        PlayerPrefs.SetInt("KeyW", (int)customInput.keyW);
        PlayerPrefs.SetInt("KeyE", (int)customInput.keyE);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("KeyQ"))
            customInput.keyQ = (KeyCode)PlayerPrefs.GetInt("KeyQ");

        if (PlayerPrefs.HasKey("KeyW"))
            customInput.keyW = (KeyCode)PlayerPrefs.GetInt("KeyW");

        if (PlayerPrefs.HasKey("KeyE"))
            customInput.keyE = (KeyCode)PlayerPrefs.GetInt("KeyE");
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}

[Serializable]
public class CustomInput
{
    public KeyCode keyQ = KeyCode.Q;
    public KeyCode keyW = KeyCode.W;
    public KeyCode keyE = KeyCode.E;
}
