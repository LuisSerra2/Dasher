using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeGetEnemies : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out EnemyController enemy))
        {
            enemy.OnEnemyDeathEffect();
        }
    }
}
