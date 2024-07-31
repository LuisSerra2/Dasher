using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour, IGameStateController
{
    public GameObject[] enemies;
    public BoxCollider[] spawnArea;
    public int numberOfEnemiesToSpawn;
    public GameObject spawnindicatorPrefab;

    public SpawnIndicator spawnIndicator;

    private float timer;
    private const float defaultTimer = 5;

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

            Instantiate(enemy, spawnPosition, Quaternion.identity);

            if (spawnIndicator != null)
            {
                spawnIndicator.ShowIndicator(spawnindicatorPrefab, spawnPosition);
            }
        }
    }

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
