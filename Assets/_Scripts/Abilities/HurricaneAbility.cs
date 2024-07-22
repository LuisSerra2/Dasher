using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneAbility : AbilityEffect
{
    public override void ApplyEffect(PlayerController player)
    {
        player.IncrementAbilityUse("HurricaneAbility");
        Destroy(gameObject);
    }
}
