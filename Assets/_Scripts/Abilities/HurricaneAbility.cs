using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneAbility : Ability
{
    public override void ApplyEffect(PlayerController player)
    {
        player.IncrementAbilityUse("HurricaneAbility");
        Destroy(gameObject);
    }
}
