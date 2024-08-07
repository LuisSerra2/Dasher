using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int HighScore;
    public KeyCode ability1;
    public KeyCode ability2;
    public KeyCode ability3;


    public GameData()
    {
        //Score
        this.HighScore = 0;

        //Inputs
        this.ability1 = KeyCode.Q;
        this.ability2 = KeyCode.W;
        this.ability3 = KeyCode.E;
    }
}

