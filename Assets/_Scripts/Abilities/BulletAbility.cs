using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffect : MonoBehaviour
{
    public abstract void ApplyEffect(PlayerController player);
}

public class BulletAbility : AbilityEffect
{
    public override void ApplyEffect(PlayerController player)
    {
        player.IncrementAbilityUse("BulletAbility");
        Destroy(gameObject);
    }
}
