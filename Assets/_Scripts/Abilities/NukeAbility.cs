using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NukeAbility : Ability
{
    public override void ApplyEffect(PlayerController player)
    {
        player.IncrementAbilityUse("NukeAbility");
        Destroy(gameObject);
    }
}
