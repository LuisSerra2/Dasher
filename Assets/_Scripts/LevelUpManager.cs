using System;
using UnityEngine;

public class LevelUpManager : Singleton<LevelUpManager>
{
    public event Action<int> OnXPReceive;
    public event Action OnLevelUp;

    private const int defaultXP = 0;
    private int XP;
    private int previousXP;
    private int currentXPMaximum = 100;
    private int upgradeCurrentXPMaximum = 2;
    private int previousXPMaximum;
    private int LevelUpCount;

    [HideInInspector]
    public int bossLevel;

    private void Start()
    {
        XP = defaultXP;
        LevelUpCount = 1;
        bossLevel = 1;
        UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);
    }

    private void Update()
    {
        AddLevel();
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
        XP += EnemyXP;
        UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);
    }

    public void AddLevel()
    {
        if (HasLevelUp() && !OnBossLevel())
        {
            bossLevel++;
            OnLevelUp?.Invoke();
            previousXPMaximum = currentXPMaximum;
            currentXPMaximum *= upgradeCurrentXPMaximum;
            previousXP = XP;
            LevelUpCount++;
            AddXPReceivedMore();
            UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);
        }
        
    }

    //Obter se o value do slider esta no maximo

    public int GetXPAfterLevelUp()
    {
        int xp = previousXP - previousXPMaximum;

        if (xp < 0)
        {
            xp = -xp;
        }

        return xp;
    }

    public void AddXPReceivedMore()
    {
        XP = GetXPAfterLevelUp();
    }


    public bool HasLevelUp()
    {
        return XP >= currentXPMaximum;
    }

    public int GetCurrentLevelUp() => LevelUpCount;

    public bool OnBossLevel()
    {
        if (bossLevel >= 3)
        {
            return true;
        } else
        {
            return false;
        }
    }
}

