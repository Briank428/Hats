using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private bool music;
    private bool sound;

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
        music = MusicFX.music;
        sound = MusicFX.sound;
        if (!music) musicB.image.sprite = offMusic;
        if (!sound) soundB.image.sprite = offSound;
        pausePanel.gameObject.SetActive(false);
        pauseB.onClick.AddListener(OpenPause);
        playB.onClick.AddListener(ClosePause);
        menuB.onClick.AddListener(SceneChange);
        soundB.onClick.AddListener(ToggleSound);
        musicB.onClick.AddListener(ToggleMusic);
    }

    public void OpenPause() { GameManager.TogglePause();  pausePanel.gameObject.SetActive(true); pauseB.gameObject.SetActive(false);  }
    public void ClosePause() { GameManager.TogglePause();  pausePanel.gameObject.SetActive(false); pauseB.gameObject.SetActive(true);  }
    public void SceneChange() { SceneManager.LoadScene("Title"); }

    public void ToggleMusic(){
        bool temp = music;
        if (music) { music = false; musicB.image.sprite = offMusic;
            Debug.Log("pause");  kyle.GetComponent<AudioSource>().Pause(); }
        else { music = true; musicB.image.sprite = onMusic;
            Debug.Log("play");  kyle.GetComponent<AudioSource>().Play(0); }
    }
    public void ToggleSound() {
        bool temp = sound;
        if (sound) { sound = false; soundB.image.sprite = offSound; }
        else { sound = true; soundB.image.sprite = onSound; }
    }

}
