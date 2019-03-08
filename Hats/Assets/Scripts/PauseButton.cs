﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public bool music;
    public bool sound;

    public Sprite onMusic;
    public Sprite offMusic;
    public Sprite onSound;
    public Sprite offSound;

    private static List<Image> lives = new List<Image>();

    public Image life1;
    public Image life2;
    public Image life3;
    public Image life4;
    public Image life5;
    public Image life6;
    public Image life7;
    public Image life8;
    public Image life9;

    public Button pauseB;
    public Button playB;
    public Button musicB;
    public Button soundB;
    public Button menuB;
    public Image pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        lives.Add(life1); lives.Add(life2); lives.Add(life3); lives.Add(life4);
        lives.Add(life5); lives.Add(life6); lives.Add(life7); lives.Add(life8); lives.Add(life9);
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
        if (music) { music = false; musicB.image.sprite = offMusic; }
        else { music = true; musicB.image.sprite = onMusic; }
    }
    public void ToggleSound() {
        bool temp = sound;
        if (sound) { sound = false; soundB.image.sprite = offSound; }
        else { sound = true; soundB.image.sprite = onSound; }
    }
    public static List<Image> GetLives() {
        return lives;
    }
}
