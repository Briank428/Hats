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

    public static bool resetBool;
    
    private int total = 18;
    private int lineSpacing=43;

    public List<Button> buttons = new List<Button>();
    private List<Achievements> achieve;
    public Text title;
    public Button startB;
    public Button soundB;
    public Button musicB;
    public Button achieveB;
    public Button leaderB;
    public Button infoB;
    public Button back;
    public Button reset;
    public Image infoPanel;
    public Image leaderPanel;
    public Text leaderName;
    public Text leaderScore;
    public Image achieveP;
    public RectTransform achievePanel;
    public Text achieveText;

    public Sprite onMusic;
    public Sprite offMusic;
    public Sprite onSound;
    public Sprite offSound;

    #endregion
    void Start()
    {
        lastClicked = null;
        isOpen = false;
        if (!PlayerPrefs.HasKey("Music")) PlayerPrefs.SetInt("Music", 1);
        if (!PlayerPrefs.HasKey("Sound")) PlayerPrefs.SetInt("Sound", 1);
        infoPanel.gameObject.SetActive(false);
        leaderPanel.gameObject.SetActive(false);
        achieveP.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("Music") == 1) musicB.image.sprite = onMusic; else { musicB.image.sprite = offMusic; }
        if (PlayerPrefs.GetInt("Sound") == 1) soundB.image.sprite = onSound; else { soundB.image.sprite = offSound; }
        startB.onClick.AddListener(StartGame);
        soundB.onClick.AddListener(ToggleSound);
        musicB.onClick.AddListener(ToggleMusic);
        achieveB.onClick.AddListener(AchievePanel);
        leaderB.onClick.AddListener(LeaderPanel);
        infoB.onClick.AddListener(InfoPanel);
        back.onClick.AddListener(CloseAchieve);
        reset.onClick.AddListener(ResetAchieve);
        buttons.Add(achieveB);
        buttons.Add(leaderB);
        buttons.Add(infoB);
        Leaderboard();
        Achievements();
    }
    public void Update()
    {
        achievePanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)(achieve.Count + .5) * lineSpacing);
    }
    public void Reset() {
        int temp = buttons.IndexOf(lastClicked);
        if (isOpen && temp == 0) { achieveP.gameObject.SetActive(false); infoB.gameObject.SetActive(true);
            leaderB.gameObject.SetActive(true); achieveB.gameObject.SetActive(true); }
        else if (isOpen && temp == 1) leaderPanel.gameObject.SetActive(false);
        else if (isOpen && temp == 2) infoPanel.gameObject.SetActive(false);
        else { title.gameObject.SetActive(false); startB.gameObject.SetActive(false);
            soundB.gameObject.SetActive(false); musicB.gameObject.SetActive(false); }
    }
    public void CloseAchieve()
    {
        Reset();
        title.gameObject.SetActive(true); startB.gameObject.SetActive(true);
        soundB.gameObject.SetActive(true); musicB.gameObject.SetActive(true);
        isOpen = false;
    }
    public void AchievePanel()
    {
        Reset();
        infoB.gameObject.SetActive(false);
        achieveB.gameObject.SetActive(false);
        leaderB.gameObject.SetActive(false);
        achieveP.gameObject.SetActive(true);
        isOpen = true;
        lastClicked = achieveB;
    }
    public void LeaderPanel()
    {
        if (isOpen && lastClicked == leaderB)
        {
            leaderPanel.gameObject.SetActive(false);
            title.gameObject.SetActive(true);
            startB.gameObject.SetActive(true);
            soundB.gameObject.SetActive(true);
            musicB.gameObject.SetActive(true);
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
            soundB.gameObject.SetActive(true);
            musicB.gameObject.SetActive(true);
            isOpen = false;
        }
        else { Reset(); infoPanel.gameObject.SetActive(true); isOpen = true; }
        lastClicked = infoB;    
    }
    public void ToggleMusic()
    {
        if(PlayerPrefs.GetInt("Music") == 1)
        {
            PlayerPrefs.SetInt("Music", 0);
            musicB.image.sprite = offMusic;
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
            musicB.image.sprite = onMusic;
        }
    }
    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
            soundB.image.sprite = offSound;
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            soundB.image.sprite = onSound;
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void Leaderboard()
    {
        SaveManager saveM = new SaveManager();
        List<Leaderboard> leaderboards = saveM.saveGlob.leaderboard;
        string nameText = "TIME";
        string scoreText = "SCORE";
        foreach(Leaderboard l in leaderboards)
        {
            try
            {
                nameText += "\n\n" + l.GetName();
                scoreText += "\n\n" + l.GetScore();
            }
            catch { };
        }
        leaderName.text = nameText;
        leaderScore.text = scoreText;
    }
    public void Achievements()
    {
        SaveManager saveM = new SaveManager();
        achieve = saveM.saveGlob.completedAchievements;
        string aList;
        if (!resetBool)
        {
            aList = "<b>\nYOU HAVE " + achieve.Count + " OUT OF " + total + " ACHIEVEMENTS</b>";
            foreach (Achievements element in achieve)
            {
                aList += "\n\n\"" + element.name + "\"\n" + element.description;
            }
        }
        else { aList = "<b>\nYOU HAVE 0 OUT OF " + total + " ACHIEVEMENTS</b>"; resetBool = false; }
        achieveText.text = aList;
    }
    public void ResetAchieve()
    {
        SaveManager saveM = new SaveManager();
        saveM.saveGlob.completedAchievements = new List<Achievements>();
        saveM.saveGlob.totalAnvilsFallen = 0;
        saveM.saveGlob.totalDoctorsHats = 0;
        saveM.SaveDataToDisk();
        resetBool = true;
        Achievements();
    }
}