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
    
    public List<Button> buttons = new List<Button>();
    public Text title;
    public InputField input;
    public Button startB;
    public Button soundB;
    public Button musicB;
    public Button achieveB;
    public Button leaderB;
    public Button infoB;
    public Image infoPanel;
    public Image leaderPanel;
    public Text leaderName;
    public Text leaderScore;
    public Image achievePanel;

    public Sprite onMusic;
    public Sprite offMusic;
    public Sprite onSound;
    public Sprite offSound;

    private bool music;
    private bool sound;

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
        Leaderboard();
    }

    public void Reset() {
        int temp = buttons.IndexOf(lastClicked);
        if (isOpen && temp == 0) achievePanel.gameObject.SetActive(false);
        if (isOpen && temp == 1) leaderPanel.gameObject.SetActive(false);
        if (isOpen && temp == 2) infoPanel.gameObject.SetActive(false);
        else { title.gameObject.SetActive(false); startB.gameObject.SetActive(false);
            input.gameObject.SetActive(false);  }
    }
    public void AchievePanel()
    {
        if (isOpen && lastClicked == achieveB) {
            achievePanel.gameObject.SetActive(false);
            title.gameObject.SetActive(true);
            startB.gameObject.SetActive(true);
            input.gameObject.SetActive(true);
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
            input.gameObject.SetActive(true);
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
            input.gameObject.SetActive(true);
            isOpen = false;
        }
        else { Reset(); infoPanel.gameObject.SetActive(true); isOpen = true; }
        lastClicked = infoB;
    }
    public void ToggleMusic()
    {
        bool temp = music;
        if (music)
        {
            music = false;
            musicB.image.sprite = offMusic;
        }
        else
        {
            music = true;
            musicB.image.sprite = onMusic;
        }
    }
    public void ToggleSound()
    {
        bool temp = sound;
        if (sound)
        {
            sound = false;
            soundB.image.sprite = offSound;
        }
        else
        {
            sound = true;
            soundB.image.sprite = onSound;
        }
    }
    public void StartGame()
    {
        MusicFX.sound = sound; Debug.Log("Sound: " + sound);
        MusicFX.music = music; Debug.Log("Music: " + music);
        SceneManager.LoadScene("Game");
    }
    public void Leaderboard()
    {
        SaveManager saveM = new SaveManager();
        List<Leaderboard> leaderboards = saveM.saveGlob.leaderboard;
        Debug.Log("LeaderCount: " + leaderboards.Count);
        /*leaderName.text = "NAME\n\n" + leaderboards[0].GetName() + "\n\n" +
            leaderboards[1].GetName() + "\n\n" + leaderboards[2].GetName() + "\n\n" +
            leaderboards[3].GetName() + "\n\n" + leaderboards[4].GetName();
        leaderScore.text = "SCORE\n\n" + leaderboards[0].GetScore() + "\n\n" +
            leaderboards[1].GetScore() + "\n\n" + leaderboards[2].GetScore() + "\n\n" +
            leaderboards[3].GetScore() + "\n\n" + leaderboards[4].GetScore();*/
    }
}
