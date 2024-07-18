using System;
using System.Collections;
using System.Collections.Generic;
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

        Debug.Log(XP);
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
            Debug.Log("PreviousXPMaximum " + previousXPMaximum);
            Debug.Log("currentXPMaximum " + currentXPMaximum);
            Debug.Log("LevelUpCount " + LevelUpCount);
            Debug.Log(XP);
        }
    }

    public int GetXPAfterLevelUp()
    {
        int xp = previousXP - previousXPMaximum;
        if (xp < 0) xp = 0; 
        Debug.Log("GetXPAfterLevelUp " + xp);

        return xp;
    }

    public void AddXPReceivedMore()
    {
        XP = GetXPAfterLevelUp();
        Debug.Log("AddXPReceivedMore " + XP);
    }


    public bool HasLevelUp()
    {
        return XP >= currentXPMaximum;
    }
}
