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
    public float music;
    public float sound;
    public bool musicON;
    public bool soundON;


    public GameData()
    {
        //Score
        this.HighScore = 0;

        //Inputs
        this.ability1 = KeyCode.Q;
        this.ability2 = KeyCode.W;
        this.ability3 = KeyCode.E;

        //Sound Slider
        this.music = 0.2f;
        this.sound = 0.2f;

        //Sound Toggle
        this.musicON = true;
        this.soundON = true;
    }
}

