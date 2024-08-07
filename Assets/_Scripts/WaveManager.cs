using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : Singleton<WaveManager>, IGameStateController
{
    public GameObject[] enemies;
    public BoxCollider[] spawnArea;
    public int numberOfEnemiesToSpawn;
    public GameObject spawnindicatorPrefab;
    public List<Color> indicatorColors;

    public SpawnIndicator spawnIndicator;

    private float timer;
    private const float defaultTimer = 5;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int currentLevel = 1;

    private bool removable = true;

    private bool playerColorChange;

    private void OnEnable()
    {
        LevelUpManager.Instance.OnLevelUp += UpdateDifficulty;
    }

    private void OnDisable()
    {
        LevelUpManager.Instance.OnLevelUp -= UpdateDifficulty;
    }

    private void Start()
    {
        timer = 0;
    }

    public void Idle()
    {

    }

    public void Playing()
    {
        Timer();
    }

    public void Dead()
    {

    }

    private void Timer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = defaultTimer;
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            GameObject enemy = enemies[Random.Range(0, enemies.Length)];

            BoxCollider area = spawnArea[Random.Range(0, spawnArea.Length)];

            Vector3 spawnPosition = GetRandomPositionInArea(area);

            GameObject enemyClone = Instantiate(enemy, spawnPosition, Quaternion.identity);

            spawnedEnemies.Add(enemyClone);

            if (spawnIndicator != null)
            {
                spawnIndicator.ShowIndicator(spawnindicatorPrefab, spawnPosition, indicatorColors);
            }

            UpdateEnemyDifficulty(enemyClone);
        }
    }

    private void UpdateDifficulty()
    {
        currentLevel = LevelUpManager.Instance.GetCurrentLevelUp();
        UpdateAllEnemiesDifficulty();
    }

    private void UpdateAllEnemiesDifficulty()
    {
        foreach (var enemy in spawnedEnemies)
        {
            UpdateEnemyDifficulty(enemy);
        }
    }

    private void UpdateEnemyDifficulty(GameObject enemy)
    {
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = 10 + (currentLevel - 1) * 2;
            navMeshAgent.acceleration = 20 + (currentLevel - 1) * 20;
        }
    }

    public void RemoveEnemyFromList(GameObject gameObject)
    {
        spawnedEnemies.Remove(gameObject);
    }

    public void RemoveEnemies()
    {
        if (!removable) return;
        removable = false;
        StartCoroutine(RemoveEnemiesInGame());
    }

    IEnumerator RemoveEnemiesInGame()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (var item in spawnedEnemies)
        {
            yield return new WaitForSeconds(0.2f);
            Destroy(item.gameObject);
        }
        RandomGroundColor.Instance.ChangeGroundColorOnDeath(FindObjectOfType<PlayerController>().transform.position);
        yield return new WaitForSeconds(0.5f);
        CameraManager.Instance.ChangeCamera();
        yield return new WaitForSeconds(1f);
        playerColorChange = true;

    }

    public bool PlayerColorChange() => playerColorChange;

    Vector3 GetRandomPositionInArea(BoxCollider area)
    {
        Vector3 min = area.bounds.min;
        Vector3 max = area.bounds.max;

        return new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }
}
