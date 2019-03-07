﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public bool music;
    public bool sound;
    public Button pauseB;
    public Button playB;
    public Button musicB;
    public Button soundB;
    public Button menuB;
    public Image pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        pausePanel.gameObject.SetActive(false);
        pauseB.onClick.AddListener(OpenPause);
        playB.onClick.AddListener(ClosePause);
    }

    public void OpenPause() { GameManager.TogglePause();  pausePanel.gameObject.SetActive(true); }
    public void ClosePause() { GameManager.TogglePause();  pausePanel.gameObject.SetActive(false); }

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
        if (music) music = false;
        else music = true;
    }
    public void ToggleSound() {
        bool temp = sound;
        if (sound) sound = false;
        else sound = true;
    }
}
