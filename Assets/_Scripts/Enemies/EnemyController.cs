using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IGameStateController
{
    public GameObject enemySplitPrefab;
    public ParticleSystem enemyDeathEffectParticle;

    private PlayerController playerController;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        GameController.Instance.RegisterController(this);
    }

    public void Idle()
    {

    }
    public void Playing()
    {
        if (agent != null)
        {
            agent.SetDestination(playerController.transform.position);
        }
    }
    public void Dead()
    {
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }

    public void OnEnemyDeathEffect()
    {
        Instantiate(enemyDeathEffectParticle, transform.position, enemyDeathEffectParticle.transform.rotation);
        Instantiate(enemySplitPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
