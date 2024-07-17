using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplit : MonoBehaviour
{
    public float explosionForce = 10f;
    public float explosionRadius = 5f; 
    public float upwardsModifier = 1f;

    private void Start()
    {
        GetAllObjects();
        Destroy(gameObject, 5f);
    }

    private void GetAllObjects()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
        }
    }
}
