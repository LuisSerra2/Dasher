using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public MusicType Type;

    private void Start()
    {
        SoundManager.PlaySound(Type);
    }
}
