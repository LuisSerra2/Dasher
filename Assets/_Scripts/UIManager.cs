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

    private Coroutine lerpCoroutine;

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

}
