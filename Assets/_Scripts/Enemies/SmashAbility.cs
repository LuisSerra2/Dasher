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
            }
        }
    }

    private void Smash()
    {
        canSmash = true;
        rndZone = Random.Range(0, noZone.transform.childCount);
        noZone.transform.GetChild(rndZone).gameObject.SetActive(true);
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
            LauchCube();
        });
    }

    private void LauchCube() {

        Vector3 direction = (PlayerController.Instance.transform.position - transform.position).normalized;

        rb.AddForce(direction * launchForce);

        Destroy(gameObject, 2f);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out PlayerController player))
        {
            player.ChangeStateOnDeath();
        }
    }
}
