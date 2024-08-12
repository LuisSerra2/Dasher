using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum CustomDataEnum {
    CustomDataPosition,
    CustomDataRotation,
}


public class UIGameData : MonoBehaviour {
    public Dictionary<CustomDataEnum, TesteIndex> customDataDictionary;

    public Button StartButton;
    public Button ExitButton;

    public Button ReturnToMainMenu;

    public List<CustomData> uiData;

    private void Start() {
        // Inicialize o Dictionary mapeando CustomDataEnum para os CustomData correspondentes
        customDataDictionary = new Dictionary<CustomDataEnum, TesteIndex>();
        for (int i = 0; i < uiData.Count; i++) {
            CustomDataEnum enumKey = (CustomDataEnum)i;
            customDataDictionary.Add(enumKey, uiData[i].testeIndex);
        }

        TesteIndex testeIndex = customDataDictionary[CustomDataEnum.CustomDataPosition];
    }
}

[Serializable]
public class CustomData {

    public string _name;
    public string name { get { return _name; } set { _name = name; } }

    [Space]

    public bool _inCenter = false;
    public bool inCenter { get { return _inCenter; } set { _inCenter = value; } }

    public bool _FirstPanelYouNeedToUseControlToActivate = false;
    public bool FirstPanelYouNeedToUseControlToActivate { get { return _FirstPanelYouNeedToUseControlToActivate; } set { _FirstPanelYouNeedToUseControlToActivate = value; } }


    [Space]

    public RectTransform _painel;
    public RectTransform painel { get { return _painel; } set { _painel = painel; } }

    [Space]

    public KeyCode _key;
    public KeyCode key { get { return _key; } set { _key = key; } }

    [Space]

    public List<ButtonData> buttonDatas;

    [Header("Animation")]

    public TesteIndex testeIndex;

    public enum ChangeAnimation {
        Position,
        Scale
    }

    [Space]

    public ChangeAnimation _ChangeAnimation;

    [Space]

    public float duration;

    [Space]

    public Ease animationStyle;
}

[Serializable]
public class ButtonData {
    public string name;
    public Button button;
    public int buttonIndex;
}

[Serializable]
public class TesteIndex {
    public Vector3 painelStartPosition;
    [Space]
    public Vector3 painelAnimIn;
    [Space]
    public Vector3 painelAnimOut;
}
