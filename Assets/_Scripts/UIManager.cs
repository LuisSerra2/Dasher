using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI bulletAbilityText;
    public TextMeshProUGUI nukeAbilityText;
    public TextMeshProUGUI hurricaneAbilityText;

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
}
