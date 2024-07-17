using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public abstract void ApplyEffect(PlayerController player);
}

public class BulletAbility : Ability
{
    public override void ApplyEffect(PlayerController player)
    {
        player.IncrementAbilityUse("BulletAbility");
        Destroy(gameObject);
    }
}
