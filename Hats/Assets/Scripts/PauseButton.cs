using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{

    public Sprite onMusic;
    public Sprite offMusic;
    public Sprite onSound;
    public Sprite offSound;

    private static List<Image> lives = new List<Image>();

    public Player kyle;
    public Button pauseB;
    public Button playB;
    public Button musicB;
    public Button soundB;
    public Button menuB;
    public Image pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Music")) PlayerPrefs.SetInt("Music", 1);
        if (!PlayerPrefs.HasKey("Sound")) PlayerPrefs.SetInt("Sound", 1);
        if (PlayerPrefs.GetInt("Music") == 1) musicB.image.sprite = onMusic; else { musicB.image.sprite = offMusic; }
        if (PlayerPrefs.GetInt("Sound") == 1) soundB.image.sprite = onSound; else { soundB.image.sprite = offSound; }
        pausePanel.gameObject.SetActive(false);
        pauseB.onClick.AddListener(OpenPause);
        playB.onClick.AddListener(ClosePause);
        menuB.onClick.AddListener(SceneChange);

        soundB.onClick.AddListener(ToggleSound);
        musicB.onClick.AddListener(ToggleMusic);
    }

    public void OpenPause() { GameManager.TogglePause();  pausePanel.gameObject.SetActive(true); pauseB.gameObject.SetActive(false);  }
    public void ClosePause() { GameManager.TogglePause();  Debug.Log("Closing panel");
        pausePanel.gameObject.SetActive(false); pauseB.gameObject.SetActive(true);  }
    public void SceneChange() { GameManager.TogglePause();  SceneManager.LoadScene("Title"); }

    public void ToggleMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
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

}
