using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IGameStateController
{
    public GameObject enemySplitPrefab;
    public ParticleSystem enemyDeathEffectParticle;


    public int XP = 20;


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
        // Implement Idle state behavior here
    }

    public void Playing()
    {
        if (agent != null && playerController != null)
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
        LevelUpManager.Instance.StartXPEvent(XP);
        WaveManager.Instance.RemoveEnemyFromList(gameObject);
        if (enemyDeathEffectParticle != null)
        {
            Instantiate(enemyDeathEffectParticle, transform.position, enemyDeathEffectParticle.transform.rotation);
        }

        if (enemySplitPrefab != null)
        {
            Instantiate(enemySplitPrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
