using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Text title;
    public Button startB;
    public Button soundB;
    public Button musicB;
    public Button achieveB;
    public Button leaderB;
    public Button infoB;
    public Button resetB;
    public Button resetY;
    public Button resetN;
    public Button infoC;
    public Button leaderC;
    public Image resetPanel;
    public Image infoPanel;
    public Image leaderPanel;

    public bool music;
    public bool sound;

    void Start()
    {
        music = true;
        sound = true;
        resetPanel.gameObject.SetActive(false);
        infoPanel.gameObject.SetActive(false);
        leaderPanel.gameObject.SetActive(false);
        startB.onClick.AddListener(StartGame);
        soundB.onClick.AddListener(ToggleSound);
        musicB.onClick.AddListener(ToggleMusic);
        achieveB.onClick.AddListener(ShowAchieve);
        leaderB.onClick.AddListener(ShowLeader);
        infoB.onClick.AddListener(ShowInfo);
        resetB.onClick.AddListener(ResetCheck);
        resetY.onClick.AddListener(ResetYes);
        resetN.onClick.AddListener(ResetNo);
        infoC.onClick.AddListener(CloseInfo);
        leaderC.onClick.AddListener(CloseLeader);
    }

    public void ResetCheck() {
        Debug.Log("Reset Panel");
        startB.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        resetPanel.gameObject.SetActive(true);
    }

    public void ResetYes()
    {
        Debug.Log("Resetting");
        resetPanel.gameObject.SetActive(false);
        startB.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
    }
    public void ResetNo()
    {
        Debug.Log("Not Resetting");
        resetPanel.gameObject.SetActive(false);
        startB.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
    }
    public void ShowInfo()
    {
        startB.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        infoPanel.gameObject.SetActive(true);
    }
    public void CloseInfo()
    {
        startB.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        infoPanel.gameObject.SetActive(false);
    }
    public void ShowAchieve()
    {

    }
    public void ShowLeader()
    {
        startB.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        leaderPanel.gameObject.SetActive(true);
    }
    public void CloseLeader()
    {
        startB.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        infoPanel.gameObject.SetActive(false);
    }
    public void ToggleMusic()
    {
        bool temp = music;
        if (music)
            music = false;
        else
            music = true;
        Debug.Log("Music Before: " + temp + "\nMusic Now: " + music);
    }
    public void ToggleSound()
    {
        bool temp = sound;
        if (sound)
            sound = false;
        else
            sound = true;
        Debug.Log("Sound Before: " + temp + "\nSound Now: " + sound);
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
