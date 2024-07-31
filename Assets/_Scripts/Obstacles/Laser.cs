using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laserCube;
    public ParticleSystem laserEffect;
    public int amountCubes;
    public int spacing;
    public float delay = 0.1f;

    public bool isHorizontal;

    public Queue<GameObject> stack = new Queue<GameObject>();

    private void Start()
    {
        InstantiateLaser();
    }

    private void InstantiateLaser()
    {
        StartCoroutine(LaserAnimation());
    }

    IEnumerator LaserAnimation()
    {
        yield return new WaitForSeconds(2);

        Vector3 newPosition;

        for (int i = 0; i < amountCubes; i++)
        {
            if (isHorizontal)
            {
                newPosition = transform.position + new Vector3(i * spacing, 0, 0);
            } else
            {
                newPosition = transform.position - new Vector3(0, 0, i * spacing);
            }

            GameObject laserCubeClone = Instantiate(laserCube, newPosition, Quaternion.identity, transform);
            Instantiate(laserEffect, laserCubeClone.transform.position, Quaternion.identity, laserCubeClone.transform);
            stack.Enqueue(laserCubeClone);

            yield return new WaitForSeconds(delay);
        }

        while (stack.Count > 0)
        {
            Destroy(stack.Dequeue());
            yield return new WaitForSeconds(delay);
        }

        if (stack.Count <= 0)
        {
            Destroy(gameObject);
        }
    }
}
