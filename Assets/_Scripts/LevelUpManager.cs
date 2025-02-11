using System;
using UnityEngine;

public class LevelUpManager : Singleton<LevelUpManager>
{
    public event Action<int> OnXPReceive;
    public event Action OnLevelUp;
    public event Action OnBossIncoming;
    public event Action OnBossDefeated;

    private const int defaultXP = 0;
    private int XP;
    private int previousXP;
    private int currentXPMaximum = 100;
    private int upgradeCurrentXPMaximum = 2;
    private int previousXPMaximum;
    private int LevelUpCount;

    private int bossSpawnFrequency = 2;
    private bool stopXP = false;

    private void Start()
    {
        XP = defaultXP;
        LevelUpCount = 1;
        //UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);
    }

    private void Update()
    {
        CheckLevelUp();
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

            LevelUp();
        }
    }

    private void LevelUp()
    {
        OnLevelUp?.Invoke();
        previousXPMaximum = currentXPMaximum;
        currentXPMaximum *= upgradeCurrentXPMaximum;
        previousXP = XP;
        LevelUpCount++;
        XP = GetXPAfterLevelUp();
        UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);
    }

    public void BossDefeated()
    {
        stopXP = false;
        LevelUpCount++;
        previousXPMaximum = currentXPMaximum;
        currentXPMaximum *= upgradeCurrentXPMaximum;
        XP = 0;
        UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);
        OnBossDefeated?.Invoke();
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
}
