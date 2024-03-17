using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager1 : MonoBehaviour
{
    public GameObject music;

    public void Awake()
    {
        music.SetActive(true);
    }

    public void Mute()
    {
        music.SetActive(false);
        
    }

    public void Unmute()
    {
        music.SetActive(true);
    }
}
