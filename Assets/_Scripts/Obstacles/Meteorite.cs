using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    public float speed = 5;
    bool canMove = false;

    [HideInInspector]
    public GameObject warningObject;

    private void Start()
    {
        StartCoroutine(Move());
    }

    private void Update()
    {
        if (canMove)
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(1);

        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            if (warningObject != null)
            {
                Destroy(warningObject);
            }

            CameraShake.Instance.ShakeCamera(0.2f, 2);
            Destroy(gameObject);
        }
    }
}
