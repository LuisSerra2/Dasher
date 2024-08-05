using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGroundColor : Singleton<RandomGroundColor>
{
    public float explosionCenter;
    public int blockChangeQuantity;

    public GameObject[] groundCubes;
    public Color[] colors;

    private const float timerDefault = 3f;
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private List<GameObject> selectedCubes = new List<GameObject>();

    public bool canSwapColor = false;
    public float defaultTimer;
    private float timer;

    private void Start()
    {
        timer = defaultTimer;
        GetCubes();
        ColorCubes();
        StoreOriginalColors();
    }

    private void Update()
    {
        if (canSwapColor)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = defaultTimer;
                GetCubes();
                ColorCubes();
            }
        }
    }

    private void GetCubes()
    {
        selectedCubes.Clear();
        int numCubesToSelect = Mathf.Min(groundCubes.Length / blockChangeQuantity, groundCubes.Length);

        HashSet<int> selectedIndices = new HashSet<int>();

        while (selectedIndices.Count < numCubesToSelect)
        {
            int rndIndex = Random.Range(0, groundCubes.Length);
            if (selectedIndices.Add(rndIndex))
            {
                selectedCubes.Add(groundCubes[rndIndex]);
            }
        }
    }

    private void StoreOriginalColors()
    {
        foreach (GameObject go in groundCubes)
        {
            MeshRenderer renderer = go.GetComponent<MeshRenderer>();
            if (renderer != null && !originalColors.ContainsKey(go))
            {
                originalColors[go] = renderer.material.color;
            }
        }
    }

    private void ColorCubes()
    {
        foreach (GameObject go in selectedCubes)
        {
            int rndColor = Random.Range(0, colors.Length);
            MeshRenderer renderer = go.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material.color = colors[rndColor];
            }
        }
    }

    public void ChangeColorTemporary(GameObject[] cubes, Color newColor, GameObject splitCube)
    {
        StartCoroutine(ChangeColorAndRestore(cubes, newColor, splitCube));
    }

    private IEnumerator ChangeColorAndRestore(GameObject[] cubes, Color newColor, GameObject splitCube)
    {
        Dictionary<GameObject, Color> previousColors = new Dictionary<GameObject, Color>();

        foreach (GameObject cube in cubes)
        {
            MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                if (!previousColors.ContainsKey(cube))
                {
                    previousColors[cube] = renderer.material.color;
                }
                renderer.material.color = newColor;
            }
        }

        yield return new WaitForSeconds(timerDefault);

        List<GameObject> sortedCubes = new List<GameObject>(cubes);
        sortedCubes.Sort((a, b) => Vector3.Distance(b.transform.position, splitCube.transform.position).CompareTo(Vector3.Distance(a.transform.position, splitCube.transform.position)));

        foreach (GameObject cube in sortedCubes)
        {
            MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
            if (renderer != null && originalColors.ContainsKey(cube))
            {
                renderer.material.color = originalColors[cube];
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
