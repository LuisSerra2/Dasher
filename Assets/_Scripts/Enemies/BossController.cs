using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    None,
    Intro,
    Attack1,
    Attack2,
    Attack3,
    End
}

public class BossController : Singleton<BossController>, IGameStateController
{
    public BossState BossState;

    private bool canSpawn = false;

    [Header("Attack1")]
    public GameObject[] groundCube;
    public float lerpDuration = 2f;

    private List<GameObject> selectedCubes = new List<GameObject>();
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

    [Header("Attack2")]
    public GameObject Smash;
    public GameObject SmashPosition;

    public void Idle()
    {
        ChangeState(BossState.None);
    }

    public void Playing()
    {
        switch (BossState)
        {
            case BossState.None:
                if (!LevelUpManager.instance.OnBossLevel()) return;

                BossState nextAttack = Random.Range(0, 2) == 0? BossState.Attack1 : BossState.Attack2;
                Debug.Log(nextAttack);

                ChangeState(nextAttack);
                break;
            case BossState.Attack1:
                if (!WaveManager.Instance.WaitAllEnemiesHasDied()) return;
                Attack1();
                break;
            case BossState.Attack2:
                if (!WaveManager.Instance.WaitAllEnemiesHasDied()) return;
                Attack2();
                break;
        }
    }

    public void Dead()
    {
        ChangeState(BossState.None);
        LevelUpManager.Instance.BossDefeated();
    }

    public void ChangeState(BossState bossState)
    {
        canSpawn = true;
        BossState = bossState;
    }

    public void Attack1()
    {
        if (!canSpawn) return;
        canSpawn = false;
        StartCoroutine(ChangeCubeColorLerp());
    }

    public void Attack2()
    {
        if (!canSpawn) return;
        canSpawn = false;
        Instantiate(Smash, SmashPosition.transform.position, Quaternion.identity);
    }

    IEnumerator ChangeCubeColorLerp()
    {
        int totalCubes = groundCube.Length;
        int minCubes = totalCubes / 3;
        int maxCubes = totalCubes / 2;
        int cubeCounts = Random.Range(minCubes, maxCubes);

        HashSet<int> selectedIndexes = new HashSet<int>();
        selectedCubes.Clear();
        originalColors.Clear();
        originalPositions.Clear();

        while (selectedIndexes.Count < cubeCounts)
        {
            int randomIndex = Random.Range(0, totalCubes);
            if (!selectedIndexes.Contains(randomIndex))
            {
                selectedIndexes.Add(randomIndex);
                GameObject cube = groundCube[randomIndex];
                selectedCubes.Add(cube);

                if (cube != null)
                {
                    Renderer renderer = cube.GetComponent<Renderer>();
                    if (renderer != null && !originalColors.ContainsKey(cube))
                    {
                        originalColors[cube] = renderer.material.color;
                    }

                    if (!originalPositions.ContainsKey(cube))
                    {
                        originalPositions[cube] = cube.transform.position;
                    }
                }
            }
        }

        float elapsedTime = 0;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpDuration;

            foreach (GameObject cube in selectedCubes)
            {
                if (cube != null)
                {
                    Renderer renderer = cube.GetComponent<Renderer>();
                    renderer.material.color = Color.Lerp(renderer.material.color, Color.white, t);
                }
            }

            yield return null;
        }

        foreach (GameObject cube in selectedCubes)
        {
            if (cube != null)
            {
                cube.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        OnColorChangeComplete();
    }

    private void OnColorChangeComplete()
    {
        StartCoroutine(AnimateCubesJump());
    }

    private IEnumerator AnimateCubesJump()
    {

        foreach (GameObject cube in selectedCubes)
        {
            if (cube != null)
            {
                cube.AddComponent<CubeAttack>();

                Vector3 cubeEndPos = new Vector3(cube.transform.position.x, cube.transform.position.y + 2, cube.transform.position.z);

                cube.transform.DOMove(cubeEndPos, 0.2f).OnComplete(() =>
                {
                    cube.transform.DOMove(originalPositions[cube], 0.2f).OnComplete(() =>
                    {
                        Destroy(cube.GetComponent<CubeAttack>());
                    });
                });

            }
        }

        Dead();

        yield return new WaitForSeconds(1);

        StartCoroutine(RestoreOriginalColors());
    }

    private IEnumerator RestoreOriginalColors()
    {
        yield return new WaitForSeconds(1f);

        float restoreDuration = 2f;
        float elapsedTime = 0;

        while (elapsedTime < restoreDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / restoreDuration;

            foreach (GameObject cube in selectedCubes)
            {
                if (cube != null && originalColors.ContainsKey(cube))
                {
                    Renderer renderer = cube.GetComponent<Renderer>();
                    Color originalColor = originalColors[cube];

                    renderer.material.color = Color.Lerp(Color.white, originalColor, t);
                }
            }

            yield return null;
        }

        foreach (GameObject cube in selectedCubes)
        {
            if (cube != null && originalColors.ContainsKey(cube))
            {
                cube.GetComponent<Renderer>().material.color = originalColors[cube];
            }
        }
    }
}
