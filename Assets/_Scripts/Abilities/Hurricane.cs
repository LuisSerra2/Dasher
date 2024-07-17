using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurricane : MonoBehaviour
{
    public float explosionLenght = 5;
    public float duration = 1;
    public float speed = 1;
    public float rotaionSpeed = 100;

    public float cameraShakeIntensity;
    public float ShakeTime;

    private void Start()
    {
        GetAllObjectInArea();
    }

    private void Update()
    {
        transform.Rotate(transform.rotation.x, transform.rotation.y + rotaionSpeed * Time.deltaTime, transform.rotation.z) ;
    }

    private void GetAllObjectInArea()
    {
        StartCoroutine(HurricaneAnimation());
    }

    IEnumerator HurricaneAnimation()
    {
        StartCoroutine(ScaleLerping.Instance.Scale(transform, transform.localScale, new Vector3(explosionLenght, transform.position.y, explosionLenght), duration, speed));

        yield return new WaitForSeconds(duration / 2);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out EnemyController enemy))
        {
            enemy.OnEnemyDeathEffect();

            CameraShake.Instance.ShakeCamera(ShakeTime, cameraShakeIntensity);
        }
    }
}
