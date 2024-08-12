using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI bulletAbilityText;
    public TextMeshProUGUI nukeAbilityText;
    public TextMeshProUGUI hurricaneAbilityText;

    [Header("XP")]

    public Slider xpSlider;
    public float lerpSpeed = 5f;

    public TextMeshProUGUI levelUpText;
    public TextMeshProUGUI xpText;

    [Header("Inputs")]
    public TextMeshProUGUI ab1;
    public TextMeshProUGUI ab2;
    public TextMeshProUGUI ab3;


    public void UpdateAbilitiesIndexText(Dictionary<string, int> abilityUses)
    {
        if (abilityUses.ContainsKey("BulletAbility"))
        {
            bulletAbilityText.text = $"{abilityUses["BulletAbility"]}";
        }

        if (abilityUses.ContainsKey("NukeAbility"))
        {
            nukeAbilityText.text = $"{abilityUses["NukeAbility"]}";
        }

        if (abilityUses.ContainsKey("HurricaneAbility"))
        {
            hurricaneAbilityText.text = $"{abilityUses["HurricaneAbility"]}";
        }
    }


    public void XpUpdate(int xp, int maxValue, int levelUpCount)
    {
        xpSlider.maxValue = maxValue;
        xpSlider.value = xp;
        levelUpText.text = levelUpCount.ToString();
        xpText.text = $"{xp} / {maxValue}";
    }

    public void UpdateInputsText(KeyCode ab1, KeyCode ab2, KeyCode ab3)
    {
        if (this.ab1 == null) return;
        this.ab1.text = ab1.ToString();
        this.ab2.text = ab2.ToString();
        this.ab3.text = ab3.ToString();
    }

}
