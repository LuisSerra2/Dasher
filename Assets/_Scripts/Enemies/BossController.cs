using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Attack1")]
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject bullet;

    private int count = 0;

    private bool canSpawn = false;

    [Space(20)]

    [Header("Attack2")]
    public GameObject smashBall;
    public GameObject smashBallPosition;

    public float shakeDuration = 1f;
    public float shakeIntensity = 3f;


    public void Idle()
    {
        ChangeState(BossState.None);
    }

    public void Playing()
    {
        switch (BossState)
        {
            case BossState.None:
                if (Input.GetKeyDown(KeyCode.K))
                {
                    ChangeState(BossState.Attack2);
                }
                break;
            case BossState.Intro:
                break;
            case BossState.Attack1:
                Attack1();
                break;
            case BossState.Attack2:
                Attack2();
                break;
            case BossState.Attack3:
                Attack3();
                break;
            case BossState.End:
                break;
            default:
                break;
        }
    }
    public void Dead()
    {
        ChangeState(BossState.None);
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
        StartCoroutine(IEAttack1());
    }

    public void Attack2()
    {
        if (!canSpawn) return;
        canSpawn = false;
        Smash();
    }
    public void Attack3()
    {

    }

    private IEnumerator IEAttack1()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject armPosition = (count % 2 == 0) ? leftArm : rightArm;

            GameObject bulletClone = Instantiate(bullet, armPosition.transform.position, Quaternion.identity);
            Shoot(bulletClone);

            count++;
            yield return new WaitForSeconds(1f);
        }
    }

    private void Shoot(GameObject bullet)
    {
        Vector3 direcao = (PlayerController.Instance.transform.position - bullet.transform.position).normalized;

        Rigidbody rb = bullet.transform.GetChild(0).GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direcao * 80f, ForceMode.Impulse); 
        }
    }

    private void Smash()
    {
        StartCoroutine(IESmash());
    }

    IEnumerator IESmash()
    {
        //Animation

        yield return new WaitForSeconds(1f);

        CameraShake.Instance.ShakeCamera(1f, 3);
        GameObject smashBallClone = Instantiate(smashBall, smashBallPosition.transform.position, Quaternion.identity);

        smashBallClone.transform.DOScale(new Vector3(100, 100, 100), 5f).OnComplete(()=> Destroy(smashBallClone));
        
        
    }
}
