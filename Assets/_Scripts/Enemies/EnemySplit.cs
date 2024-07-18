using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplit : MonoBehaviour
{
    public float explosionForce = 10f;
    public float explosionRadius = 5f; 
    public float upwardsModifier = 1f;

    public LayerMask groundLayer;

    private void Start()
    {
        GetGroundCubes(gameObject);
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

    private void GetGroundCubes(GameObject enemySplit)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, RandomGroundColor.Instance.explosionCenter, groundLayer);

        List<GameObject> groundCubes = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            groundCubes.Add(collider.gameObject);
        }

        Color newColor = Color.white;
        if (enemySplit != null)
        {
            MeshRenderer enemyRenderer = enemySplit.GetComponentInChildren<MeshRenderer>();
            if (enemyRenderer != null)
            {
                newColor = enemyRenderer.material.color;
            }
        }

        RandomGroundColor.Instance.ChangeColorTemporary(groundCubes.ToArray(), newColor, enemySplit);
    }
}
