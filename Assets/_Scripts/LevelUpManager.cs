using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class LevelUpManager : Singleton<LevelUpManager>
{
    public event Action<int> OnXPReceive;

    private const int defaultXP = 0;
    private int XP;
    private int previousXP;
    private int currentXPMaximum = 100;
    private int upgradeCurrentXPMaximum = 2;
    private int previousXPMaximum;
    private int LevelUpCount;

    private void Start()
    {
        XP = defaultXP;
        LevelUpCount = 1;
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
        if (HasLevelUp())
        {
            previousXPMaximum = currentXPMaximum;
            currentXPMaximum *= upgradeCurrentXPMaximum;
            previousXP = XP;
            LevelUpCount++;
            AddXPReceivedMore();
            UIManager.Instance.XpUpdate(XP, currentXPMaximum, LevelUpCount);
        }
    }

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

}
