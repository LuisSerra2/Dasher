using DG.Tweening;
using System;
using System.Collections;
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

    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject bullet;
    private bool canSpawn = false;

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
                if(!LevelUpManager.instance.OnBossLevel()) return;
                ChangeState(BossState.Attack2);
                break;
            case BossState.Attack1:
                Attack1();
                break;
            case BossState.Attack2:
                if (!WaveManager.Instance.WaitAllEnemiesHasDied()) return;
                Attack2();
                break;
        }
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
        Instantiate(Smash, SmashPosition.transform.position, Quaternion.identity);
    }

    public void Dead()
    {
        ChangeState(BossState.None);
        LevelUpManager.Instance.BossDefeated();
    }

    private IEnumerator IEAttack1()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject armPosition = (i % 2 == 0) ? leftArm : rightArm;
            GameObject bulletClone = Instantiate(bullet, armPosition.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }
}
