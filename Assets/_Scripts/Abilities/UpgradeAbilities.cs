using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAbilities : Singleton<UpgradeAbilities>
{
    public event Action OnUpgradeQ;
    public event Action OnUpgradeW;
    public event Action OnUpgradeE;
    public event Action OnUpgradeR;

    private QAbility qAbility;

    private void Start()
    {
        qAbility = FindObjectOfType<QAbility>();
    }

    private void Update()
    {
        UpgradeInputs(OnUpgradeQ, KeyCode.Q);
        UpgradeInputs(OnUpgradeW, KeyCode.W);
        UpgradeInputs(OnUpgradeE, KeyCode.E);
        UpgradeInputs(OnUpgradeR, KeyCode.R);
    }

    private void OnEnable()
    {
        OnUpgradeQ += UpgradeQAbility;
    }
    private void OnDisable()
    {
        OnUpgradeQ -= UpgradeQAbility;
    }

    private void UpgradeInputs(Action onUpgrade, KeyCode keyCode)
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(keyCode)){
                onUpgrade?.Invoke();
                Debug.Log("Q Ability Upgraded");
            }
        }

    }

    private void UpgradeQAbility()
    {

        qAbility.Upgrade();
    }
}

public abstract class AbilityUpgrade : MonoBehaviour
{
    public int level;
    public int maxLevel;

    public abstract void Upgrade();
}


