using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAttack : MonoBehaviour
{
    private bool canDamage = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (canDamage) return;

        if (collision.collider.TryGetComponent(out PlayerController player))
        {
            canDamage = true;
            player.ChangeStateOnDeath();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (canDamage) return;

        if (collision.collider.TryGetComponent(out PlayerController player))
        {
            canDamage = true;
            player.ChangeStateOnDeath();
        }
    }
}
