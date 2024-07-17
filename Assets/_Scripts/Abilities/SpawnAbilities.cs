using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAbilities : Singleton<SpawnAbilities>, IGameStateController
{
    private BoxCollider area;

    [SerializeField] private GameObject[] Ability;

    public float defaultTimer = 5;
    private float timer;

    private void Start()
    {
        area = GetComponent<BoxCollider>();
        timer = defaultTimer;
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

    public void Timer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = defaultTimer;
            SpawnAbility();

        }
    }

    public void SpawnAbility()
    {
        GameObject ability = Ability[Random.Range(0, Ability.Length)];

        Vector3 spawnPosition = GetRandomPositionInArea(area);

        Instantiate(ability, spawnPosition, Quaternion.identity);

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
