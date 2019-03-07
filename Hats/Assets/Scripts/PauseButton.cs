using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Button pauseB;
    public Button playB;
    public Button musicB;
    public Button soundB;
    public Image pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        //pausePanel.gameObject.SetActive(false);
        //pauseB.onClick.AddListener(OpenPause);
        //  playB.onClick.AddListener(ClosePause);
    }

    public void OpenPause() { GameManager.TogglePause();  pausePanel.gameObject.SetActive(true); }
    public void ClosePause() { GameManager.TogglePause();  pausePanel.gameObject.SetActive(false); }
}
