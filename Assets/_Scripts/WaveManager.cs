using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour, IGameStateController
{
    public static WaveManager Instance;

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
    private bool canSpawn = true; 

    private void OnEnable()
    {
        LevelUpManager.Instance.OnLevelUp += UpdateDifficulty;
        LevelUpManager.Instance.OnBossIncoming += StopSpawning;
        LevelUpManager.Instance.OnBossDefeated += ResumeSpawning;
    }

    private void OnDisable()
    {
        LevelUpManager.Instance.OnLevelUp -= UpdateDifficulty;
        LevelUpManager.Instance.OnBossIncoming -= StopSpawning;
        LevelUpManager.Instance.OnBossDefeated -= ResumeSpawning;
    }

    private void Awake()
    {
        Instance = this;
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
        if (!canSpawn) return;
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
            if (canSpawn && !LevelUpManager.Instance.OnBossLevel())
            {
                SpawnEnemies();
            }
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
            if (enemy == null) return;
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
        foreach (GameObject item in spawnedEnemies)
        {
            yield return new WaitForSeconds(0.2f);
            Destroy(item);
        }
        RandomGroundColor.Instance.ChangeGroundColorOnDeath(FindObjectOfType<PlayerController>().transform.position);
        yield return new WaitForSeconds(0.5f);
        CameraManager.Instance.ChangeCamera();
        yield return new WaitForSeconds(1f);
        playerColorChange = true;
        UIManager.Instance.EndGameMenu();
    }

    public bool PlayerColorChange() => playerColorChange;

    private Vector3 GetRandomPositionInArea(BoxCollider area)
    {
        Vector3 min = area.bounds.min;
        Vector3 max = area.bounds.max;

        return new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

    public bool WaitAllEnemiesHasDied()
    {
        return spawnedEnemies.Count <= 0;
    }

    private void StopSpawning()
    {
        canSpawn = false;
    }

    private void ResumeSpawning()
    {
        canSpawn = true;
    }
}
