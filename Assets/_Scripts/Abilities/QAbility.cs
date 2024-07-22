using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QAbility : AbilityUpgrade
{
    public float speed = 10;
    public float size;

    public override void Upgrade()
    {
        if (level < maxLevel)
        {
            level++;
            size += 50;
            speed *= 1.1f;
        }
    }
}
