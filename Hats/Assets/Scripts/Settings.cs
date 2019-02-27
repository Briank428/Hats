using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
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
    public Image resetPanel;

    public bool music;
    public bool sound;

    void Start()
    {
        music = true;
        sound = true;
        resetPanel.gameObject.SetActive(false);
        startB.onClick.AddListener(StartGame);
        soundB.onClick.AddListener(ToggleSound);
        musicB.onClick.AddListener(ToggleMusic);
        achieveB.onClick.AddListener(ShowAchieve);
        leaderB.onClick.AddListener(ShowLeader);
        infoB.onClick.AddListener(ShowInfo);
        resetB.onClick.AddListener(ResetCheck);
    }

    public void ResetCheck() {
        Debug.Log("Reset Panel");
        resetPanel.gameObject.SetActive(true);

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
