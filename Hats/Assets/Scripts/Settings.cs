using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public bool music;
    public bool sound;

    void Start()
    {
        music = true;
        sound = true;
    }
    void ToggleMusic()
    {
        if (music)
            music = false;
        else
            music = true;
    }
    void ToggleSound()
    {
        if (sound)
            sound = false;
        else
            sound = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
