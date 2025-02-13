using System;
using UnityEngine;

public class LevelUpManager : Singleton<LevelUpManager>, IGameStateController
{
    public event Action<int> OnXPReceive;
    public event Action OnLevelUp;
    public event Action OnBossIncoming;
    public event Action OnBossDefeated;

    public GameObject BWSphere;

    private const int defaultXP = 0;
    private int XP;
    private int previousXP;
    private int currentXPMaximum = 100;
    private float xpScalingFactor = 1.5f;
    private int previousXPMaximum;
    private int LevelUpCount;

    private int bossSpawnFrequency = 3;
    private bool stopXP = false;

    private void Start()
    {
        XP = defaultXP;
        LevelUpCount = 1;
    }

    public void Idle()
    {
    }

    public void Playing()
    {
        CheckLevelUp();
    }

    public void Dead()
    {
        Debug.Log("HERE");
    }

    public void OnEnable()
    {
        OnXPReceive += LevelUpManager_OnXPReceive;
    }

    private void OnDisable()
    {
        OnXPReceive -= LevelUpManager_OnXPReceive;
    }

    public void StartXPEvent(int XP)
    {
        OnXPReceive?.Invoke(XP);
    }

    private void LevelUpManager_OnXPReceive(int EnemyXP)
    {
        if (stopXP) return;

        XP += EnemyXP;
        UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);
    }

    private void CheckLevelUp()
    {
        if (HasLevelUp())
        {
            if (LevelUpCount + 1 == bossSpawnFrequency)
            {
                OnBossIncoming?.Invoke();
            }

            if (OnBossLevel())
            {
                stopXP = true;
                return;
            }

            if (OnBWSphereLevel())
            {
                Show_HideBWSphere();
            } else
            {
                Show_HideBWSphere();
            }

            LevelUp();
        }
    }

    private void LevelUp()
    {       
        OnLevelUp?.Invoke();
        previousXPMaximum = currentXPMaximum;
        currentXPMaximum = Mathf.RoundToInt(currentXPMaximum * xpScalingFactor);
        previousXP = XP;
        LevelUpCount++;
        XP = GetXPAfterLevelUp();
        UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);

        if (OnBWSphereLevel())
        {
            Show_HideBWSphere();
        }

    }

    public void BossDefeated()
    {
        stopXP = false;
        LevelUpCount++;
        previousXPMaximum = currentXPMaximum;
        currentXPMaximum = Mathf.RoundToInt(currentXPMaximum * xpScalingFactor);
        XP = 0;
        UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);
        OnBossDefeated?.Invoke();
    }

    public void Show_HideBWSphere()
    {
        if (BWSphere.activeInHierarchy)
        {
            BWSphere.SetActive(false);
        } else
        {
            BWSphere.SetActive(true);
        }

    }

    private int GetXPAfterLevelUp()
    {
        return Mathf.Max(previousXP - previousXPMaximum, 0);
    }

    private bool HasLevelUp()
    {
        return XP >= currentXPMaximum;
    }

    public int GetCurrentLevelUp() => LevelUpCount;

    public bool OnBossLevel()
    {
        return LevelUpCount % bossSpawnFrequency == 0;
    }
    public bool OnBWSphereLevel()
    {
        return LevelUpCount % 4 == 0;
    }

}
