using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputSettings : MonoBehaviour
{
    private InputsManager inputsManager;
    public TMP_Dropdown keyQDropdown;
    public TMP_Dropdown keyWDropdown;
    public TMP_Dropdown keyEDropdown;

    private void Start()
    {
        inputsManager = FindObjectOfType<InputsManager>();

        inputsManager.Load();

        PopulateDropdown(keyQDropdown);
        PopulateDropdown(keyWDropdown);
        PopulateDropdown(keyEDropdown);

        SetInitialDropdownValues();

        UIManager.Instance.UpdateInputsText(
            inputsManager.customInput.keyQ,
            inputsManager.customInput.keyW,
            inputsManager.customInput.keyE
        );

        keyQDropdown.onValueChanged.AddListener(delegate { OnKeyQChanged(); });
        keyWDropdown.onValueChanged.AddListener(delegate { OnKeyWChanged(); });
        keyEDropdown.onValueChanged.AddListener(delegate { OnKeyEChanged(); });
    }

    private void PopulateDropdown(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (key >= KeyCode.A && key <= KeyCode.Z)
            {
                options.Add(key.ToString());
            }
        }

        dropdown.AddOptions(options);
    }

    public void SetInitialDropdownValues()
    {
        SetDropdownValue(keyQDropdown, inputsManager.customInput.keyQ);
        SetDropdownValue(keyWDropdown, inputsManager.customInput.keyW);
        SetDropdownValue(keyEDropdown, inputsManager.customInput.keyE);
    }

    private void SetDropdownValue(TMP_Dropdown dropdown, KeyCode key)
    {
        string keyString = key.ToString();
        int index = dropdown.options.FindIndex(option => option.text == keyString);
        if (index >= 0)
        {
            dropdown.value = index;
            dropdown.RefreshShownValue();
        }
    }

    private void OnKeyQChanged()
    {
        KeyCode newKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyQDropdown.options[keyQDropdown.value].text);
        inputsManager.SetKeyQ(newKey);
        inputsManager.Save();
        UIManager.Instance.UpdateInputsText(inputsManager.customInput.keyQ, inputsManager.customInput.keyW, inputsManager.customInput.keyE);
    }

    private void OnKeyWChanged()
    {
        KeyCode newKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyWDropdown.options[keyWDropdown.value].text);
        inputsManager.SetKeyW(newKey);
        inputsManager.Save();
        UIManager.Instance.UpdateInputsText(inputsManager.customInput.keyQ, inputsManager.customInput.keyW, inputsManager.customInput.keyE);
    }

    private void OnKeyEChanged()
    {
        KeyCode newKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyEDropdown.options[keyEDropdown.value].text);
        inputsManager.SetKeyE(newKey);
        inputsManager.Save();
        UIManager.Instance.UpdateInputsText(inputsManager.customInput.keyQ, inputsManager.customInput.keyW, inputsManager.customInput.keyE);
    }
}