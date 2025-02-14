using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public float size;
    public float cameraShakeIntensity;
    public float ShakeTime;

    public Color[] colors;

    private Vector3 direction;
    private bool directionSet = false;

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
        directionSet = true;
    }

    //public void ApplyQAbilityProperties(QAbility qAbility)
    //{
    //    transform.localScale = new Vector3(qAbility.size, qAbility.size, qAbility.size);
    //    speed = qAbility.speed;
    //}

    private void Start()
    {
        SoundManager.PlaySound(SoundType.Bullet);
        int rndIndex = Random.Range(0, colors.Length);
        transform.GetComponent<MeshRenderer>().material.color = colors[rndIndex];
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        if (directionSet)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.parent != null && collision.collider.transform.parent.TryGetComponent(out EnemyController enemy))
        {
            enemy.OnEnemyDeathEffect();

            CameraShake.Instance.ShakeCamera(ShakeTime, cameraShakeIntensity);
        }
    }
}

public struct BulletData
{
    public Vector3 direction;

    public BulletData(Vector3 direction)
    {
        this.direction = direction;
    }
}
