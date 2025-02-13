using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToNoSmashZone : MonoBehaviour
{
    public bool noZone;
    public bool hitZone;


    private void OnCollisionEnter(Collision collision)
    {
        if (hitZone)
        {
            if (collision.collider.TryGetComponent(out PlayerController player))
            {
                player.ChangeStateOnDeath();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("noSmashZone"))
        {
            noZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("noSmashZone"))
        {
            noZone = false;
        }
    }

    public bool IsInNoZone()
    {
        return noZone;
    }
}
