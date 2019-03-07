using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    #region vars
    private bool isOpen;
    private Button lastClicked;
    
    public Text title;

    public List<Button> buttons = new List<Button>();
    
    public Button startB;
    public Button soundB;
    public Button musicB;
    public Button achieveB;
    public Button leaderB;
    public Button infoB;
    public Image infoPanel;
    public Image leaderPanel;
    public Image achievePanel;

    public bool music;
    public bool sound;

    #endregion
    void Start()
    {
        lastClicked = null;
        isOpen = false;
        music = true;
        sound = true;
        infoPanel.gameObject.SetActive(false);
        leaderPanel.gameObject.SetActive(false);
        achievePanel.gameObject.SetActive(false);
        startB.onClick.AddListener(StartGame);
        soundB.onClick.AddListener(ToggleSound);
        musicB.onClick.AddListener(ToggleMusic);
        achieveB.onClick.AddListener(AchievePanel);
        leaderB.onClick.AddListener(LeaderPanel);
        infoB.onClick.AddListener(InfoPanel);
        buttons.Add(achieveB);
        buttons.Add(leaderB);
        buttons.Add(infoB);
    }
    public void Reset() {
        int temp = buttons.IndexOf(lastClicked);
        if (isOpen && temp == 0) achievePanel.gameObject.SetActive(false);
        if (isOpen && temp == 1) leaderPanel.gameObject.SetActive(false);
        if (isOpen && temp == 2) infoPanel.gameObject.SetActive(false);
        else { title.gameObject.SetActive(false); startB.gameObject.SetActive(false); }
    }
    public void AchievePanel()
    {
        if (isOpen && lastClicked == achieveB) {
            achievePanel.gameObject.SetActive(false);
            title.gameObject.SetActive(true);
            startB.gameObject.SetActive(true);
            isOpen = false;
        }
        else { Reset(); achievePanel.gameObject.SetActive(true); isOpen = true; }
        lastClicked = achieveB;
    }
    public void LeaderPanel()
    {
        if (isOpen && lastClicked == leaderB)
        {
            leaderPanel.gameObject.SetActive(false);
            title.gameObject.SetActive(true);
            startB.gameObject.SetActive(true);
            isOpen = false;
        }
        else { Reset(); leaderPanel.gameObject.SetActive(true); isOpen = true; }
        lastClicked = leaderB;
    }
    public void InfoPanel()
    {
        if (isOpen && lastClicked == infoB)
        {
            infoPanel.gameObject.SetActive(false);
            title.gameObject.SetActive(true);
            startB.gameObject.SetActive(true);
            isOpen = false;
        }
        else { Reset(); infoPanel.gameObject.SetActive(true); isOpen = true; }
        lastClicked = infoB;
    }
    public void ToggleMusic() {
        bool temp = music;
        if (music) music = false;
        else music = true;
    }
    public void ToggleSound() {
        bool temp = sound;
        if (sound) sound = false;
        else sound = true;
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
