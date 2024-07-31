using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstaclesManager : Singleton<ObstaclesManager>, IGameStateController
{
    [Header("Lasers")]
    public GameObject laserPrefab;
    public GameObject laserSpawnIndicator;
    public BoxCollider horizontalSpawnArea;
    public BoxCollider verticalSpawnArea;
    public int laserAmount = 3;
    public float minDistanceBetweenLasers = 1f;
    public List<Vector3> spawnedPositions = new List<Vector3>();

    [Header("TimerLaser")]
    public float laserDefaultTimer;
    private float laserTimer;


    [Space(20)]

    [Header("Meteriote")]
    public GameObject meteriotePrefab;
    public GameObject meterioteWarning;
    public BoxCollider meterioteSpawnArea;
    public int meteoriteAmount = 5;

    [Header("TimerMeteorite")]
    public float meterioteDefaultTimer;
    private float meterioteTimer;

    public SpawnIndicator spawnIndicator;

    private void Start()
    {
        laserTimer = laserDefaultTimer;
        meterioteTimer = meterioteDefaultTimer;
    }

    public void Idle()
    {

    }

    public void Playing()
    {
        LaserTimer();
        MeterioteTimer();
    }

    public void Dead()
    {

    }
    #region Laser
    private void InstantiateLaser()
    {
        for (int i = 0; i < laserAmount; i++)
        {
            Vector3 spawnPosition;
            BoxCollider selectedArea;
            bool isHorizontal;
            int attempts = 0;

            if (Random.Range(0, 2) == 0)
            {
                selectedArea = horizontalSpawnArea;
                isHorizontal = true;
            } else
            {
                selectedArea = verticalSpawnArea;
                isHorizontal = false;
            }

            do
            {
                spawnPosition = GetRandomPositionInArea(selectedArea);
                attempts++;
            }
            while (!IsPositionValid(spawnPosition) && attempts < 100);

            if (attempts < 100)
            {
                GameObject laser = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
                laser.GetComponent<Laser>().isHorizontal = isHorizontal; 
                spawnedPositions.Add(spawnPosition);

                if (spawnIndicator != null)
                {
                    spawnIndicator.ShowIndicator(laserSpawnIndicator, spawnPosition);
                }
            }
        }
        ClearOccupiedPosition();
    }
    public void ClearOccupiedPosition()
    {
        spawnedPositions.Clear();
    }

    private void LaserTimer()
    {
        if (LevelUpManager.Instance.GetCurrentLevelUp() <= 2) return; 

        laserTimer -= Time.deltaTime;

        if (laserTimer <= 0)
        {
            laserTimer = laserDefaultTimer;
            InstantiateLaser();
        }

    }
    #endregion

    #region Meteorite

    IEnumerator InstantiateMeteorite()
    {
        for (int i = 0; i < meteoriteAmount; i++)
        {
            Vector3 spawnPosition;
            int attempts = 0;

            do
            {
                spawnPosition = GetRandomPositionInArea(meterioteSpawnArea);
                attempts++;
            }
            while (!IsPositionValid(spawnPosition) && attempts < 100);

            if (attempts < 100)
            {

                GameObject meterioteClone = Instantiate(meteriotePrefab, spawnPosition, meteriotePrefab.transform.rotation);
                spawnedPositions.Add(spawnPosition);

                Vector3 warningPosition = new Vector3(spawnPosition.x, 0, spawnPosition.z);
                GameObject meterioteWarningClone = Instantiate(meterioteWarning, warningPosition, Quaternion.identity);
                meterioteWarningClone.transform.localScale *= 4;

                Meteorite meteoriteScript = meterioteClone.GetComponent<Meteorite>();
                meteoriteScript.warningObject = meterioteWarningClone;
                yield return new WaitForSeconds(.1f);
            }
        }
    }

    private void MeterioteTimer()
    {
        if (LevelUpManager.Instance.GetCurrentLevelUp() <= 2) return;

        meterioteTimer -= Time.deltaTime;

        if (meterioteTimer <= 0)
        {
            meterioteTimer = meterioteDefaultTimer;
            StartCoroutine(InstantiateMeteorite());
        }
    }

    #endregion

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

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(spawnedPosition, position) < minDistanceBetweenLasers)
            {
                return false; 
            }
        }
        return true;
    }
}
