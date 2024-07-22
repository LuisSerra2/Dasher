using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NukeAbility : AbilityEffect
{
    public override void ApplyEffect(PlayerController player)
    {
        player.IncrementAbilityUse("NukeAbility");
        Destroy(gameObject);
    }
}
