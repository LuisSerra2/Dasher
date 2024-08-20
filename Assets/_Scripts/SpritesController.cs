using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Sprites
{
    Music,
    Sound
}
public enum SpritesSwitch
{
    None,
    Principal,
    Second
}

[ExecuteInEditMode]
public class SpritesController : MonoBehaviour
{
    public static SpritesController instance;

    public SpritesData[] spritesDatas;

    public List<string> gameObjectName = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameObjectName.Clear();
        for (int i = 0; i < spritesDatas.Length; i++)
        {
            if (spritesDatas[i].image == null)
            {
                Debug.LogWarning($"Image at index {i} is null.");
                continue;
            }

            gameObjectName.Add(spritesDatas[i].image.name);
        }
    }

    public static void SwitchSprite(Sprites sprites, SpritesSwitch spriteIndex)
    {
        instance.spritesDatas[(int)sprites].image.sprite = instance.spritesDatas[(int)sprites].spritesCons[(int)spriteIndex].sprites;
    }

    public void Initialize()
    {
        if (spritesDatas == null || gameObjectName == null)
        {
            Debug.LogError("spritesDatas or gameObjectName is null");
            return;
        }

        if (spritesDatas.Length != gameObjectName.Count)
        {
            Debug.LogError("Length of spritesDatas and gameObjectName do not match");
            return;
        }

        for (int i = 0; i < spritesDatas.Length; i++)
        {
            if (string.IsNullOrEmpty(gameObjectName[i]))
            {
                Debug.LogWarning($"GameObject name at index {i} is null or empty");
                continue;
            }

            GameObject targetObject = GameObject.Find(gameObjectName[i]);
            if (targetObject == null)
            {
                Debug.LogWarning($"GameObject '{gameObjectName[i]}' not found");
                continue;
            }

            Image foundImage = targetObject.GetComponent<Image>();
            if (foundImage != null)
            {
                spritesDatas[i].image = foundImage;
                Debug.Log($"Assigned {gameObjectName[i]} to spritesDatas[{i}].image");
            } else
            {
                Debug.LogWarning($"Image component not found on GameObject '{gameObjectName[i]}'");
            }
        }
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] name = Enum.GetNames(typeof(Sprites));
        string[] switchSprites = Enum.GetNames(typeof(SpritesSwitch));
        Array.Resize(ref spritesDatas, name.Length);

        for (int i = 0; i < name.Length; i++)
        {
            spritesDatas[i].name = name[i];
            Array.Resize(ref spritesDatas[i].spritesCons, switchSprites.Length);

            for (int j = 0; j < switchSprites.Length; j++)
            {
                spritesDatas[i].spritesCons[j].name = switchSprites[j];
            }
        }
    }
#endif
}

[System.Serializable]
public struct SpritesData
{
    [HideInInspector] public string name;
    public Image image;
    public SpritesCon[] spritesCons;
}

[System.Serializable]
public struct SpritesCon
{
    [HideInInspector] public string name;
    public Sprite sprites;
}
