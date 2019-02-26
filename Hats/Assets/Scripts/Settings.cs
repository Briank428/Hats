using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings
{
    public Button startB;
    public Button soundB;
    public Button musicB;
    public Button achieveB;
    public Button leaderB;
    public Button infoB;
    public Button resetB;
    public Button resetY;
    public Button resetN;

    public bool music;
    public bool sound;

    void Start()
    {
        music = true;
        sound = true;
        startB.onClick.AddListener(StartGame);
        soundB.onClick.AddListener(ToggleSound);
        musicB.onClick.AddListener(ToggleMusic);
        achieveB.onClick.AddListener(ShowAchieve);
        leaderB.onClick.AddListener(ShowLeader);
        infoB.onClick.AddListener(ShowInfo);
        resetB.onClick.AddListener(ResetCheck);
        resetY.onClick.AddListener(ResetYes);
        resetN.onClick.AddListener(ResetNo);
    }

    public void ResetCheck()
    {
        
    }
    public void ResetYes()
    {

    }
    public void ResetNo()
    {

    }
    public void ShowInfo()
    {

    }
    public void ShowAchieve()
    {

    }
    public void ShowLeader()
    {

    }
    public void ToggleMusic()
    {
        if (music)
            music = false;
        else
            music = true;
    }
    public void ToggleSound()
    {
        if (sound)
            sound = false;
        else
            sound = true;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
