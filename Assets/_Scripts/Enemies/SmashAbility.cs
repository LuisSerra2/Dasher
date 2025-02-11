using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashAbility : MonoBehaviour
{
    public GameObject smashBall;
    public int smashDurationTimer = 10;

    public GameObject noZone;
    private int rndZone;

    public float shakeDuration = 1f;
    public float shakeIntensity = 3f;

    public float launchForce = 500f;

    private bool canSmash;

    private Rigidbody rb;
    private GameObject noZoneClone;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Smash();
    }

    private void Update()
    {
        if (!canSmash) return;
        Raycast();
    }

    private void Raycast()
    {
        Vector3 origin = transform.position;

        RaycastHit hit;

        if (Physics.Raycast(origin, Vector3.down, out hit, 20f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                canSmash = false;
                SmashAfterShock();
                Destroy(gameObject);
            }
        }
    }

    private void Smash()
    {
        if (noZoneClone == null)
        {
            noZoneClone = Instantiate(noZone, BossController.Instance.transform);
        }

        canSmash = true;

        for (int i = 0; i < noZoneClone.transform.childCount; i++)
        {
            noZoneClone.transform.GetChild(rndZone).gameObject.SetActive(false);
        }

        rndZone = Random.Range(0, noZoneClone.transform.childCount);
        noZoneClone.transform.GetChild(rndZone).gameObject.SetActive(true);
    }


    private void SmashAfterShock()
    {
        CameraShake.Instance.ShakeCamera(1f, 3);
        GameObject smashBallClone = Instantiate(smashBall, transform.position, Quaternion.identity);
        

        smashBallClone.transform.DOScale(new Vector3(100, 100, 100), smashDurationTimer).OnComplete(() =>
        {
            noZone.transform.GetChild(rndZone).gameObject.SetActive(false);
            BossController.Instance.ChangeState(BossState.None);
            Destroy(smashBallClone);
        });
        BossController.Instance.Dead();
        Destroy(noZoneClone, 5f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out PlayerController player))
        {
            player.ChangeStateOnDeath();
        }
    }
}
