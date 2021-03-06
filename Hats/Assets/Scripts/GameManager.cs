﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    #region public vars
    [SerializeField]
    public List<Hat> hatTypes = new List<Hat>();
    public Player playerPrefab;
    public Transform spawnPoint;
    public Camera camera1;
    public static GameManager gm;
    public Text counter;
    public Text life;
    public Text countdown;
    public List<Image> lives;
    public int numHatsCollected;
    public AudioClip main, secondary;
    #endregion

    #region consts
    private const float SPEED_INIT = 0.8f; //initial time between drops
    private const float MAX_SPEED = 0.20f; //max hat drop speed
    private const float SPEED_DECREMENT = 0.05f;
    #endregion

    #region private vars
    private static SaveManager saveManager;
    private float currentSpeed; //stores current time between drops
    private Player playerInstance;
    [SerializeField]
    private int numLives;
    private static List<Achievements> achievements;
    private List<Leaderboard> leaderboard;
    private bool gamePlaying;
    private bool paused;
    private List<string> hatsCollected;
    
    private KeyCode[] konami = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
    };
    private int sequenceIndex;
    #endregion

    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        counter.text = "0";
        gm = this;
        countdown.gameObject.SetActive(false);
        saveManager = new SaveManager();
        achievements = saveManager.saveGlob.completedAchievements;
        leaderboard = saveManager.saveGlob.leaderboard;
        numLives = 9;
        currentSpeed = SPEED_INIT;
        StartCoroutine("StartGame");
        gamePlaying = true;
        paused = false;
        foreach (Image i in lives) i.gameObject.SetActive(true);
        hatsCollected = new List<string>();
        GetComponent<AudioSource>().clip = main;
        GetComponent<AudioSource>().Play();

    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(konami[sequenceIndex]))
        {
            if (++sequenceIndex == konami.Length)
            {
                sequenceIndex = 0;
                achievementsAdd("Old Timer", "Enter the Konami Code");
                if (PlayerPrefs.GetInt("Music") == 1)
                {
                    GetComponent<AudioSource>().clip = secondary;
                    GetComponent<AudioSource>().Play();
                }
            }
        }
        else if (Input.anyKeyDown) sequenceIndex = 0;

        if(PlayerPrefs.GetInt("Music") == 1 && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().UnPause();
        }

        if(PlayerPrefs.GetInt("Music") == 0 && GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Pause();
        }
        

    }

    IEnumerator StartGame()
    {
        //spawn player
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.position = new Vector2(0, -13);
        playerInstance.gameObject.GetComponent<MouseMove2D>().enabled = false;

        //Countdown
        countdown.gameObject.SetActive(true);
        for (int i = 3; i >= 1; i--)
        {
            Debug.Log(i); //insert countdown here
            countdown.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdown.gameObject.SetActive(false);
        Debug.Log("Start!"); //insert Start! here
        countdown.text = "Start!";
        yield return new WaitForSeconds(1);
        countdown.text = "";

        playerInstance.gameObject.GetComponent<MouseMove2D>().enabled = true;
        while (gamePlaying)
        {
            yield return new WaitForSeconds(currentSpeed);
            SpawnHat();
        }
        
    }

    void SpawnHat() //spawns hats and anvils (anvils are a type of hat for simplicity)
    {
        //choose hat to spawn
        Hat temp;
        do
        {
            temp = hatTypes[random.Next(hatTypes.Count)];
        } while (random.Next(0, 100)/100.0 < temp.skipProbability);

        //spawn hat

        Hat temp2 = Instantiate(temp) as Hat;
        float width = random.Next(-5, 5);
        spawnPoint.position = new Vector2(width, spawnPoint.position.y);
        temp2.transform.position = spawnPoint.position;
    }

    public void HatCollected(Hat hat)
    {
        hatsCollected.Add(hat.name);
        numHatsCollected++;
        counter.text = "" + numHatsCollected;
        spawnPoint.position += new Vector3(0, 2 * hat.height, 0);
        camera1.transform.position += new Vector3(0, 2 * hat.height, 0);
        if (numHatsCollected % 10 == 0 && currentSpeed > MAX_SPEED + SPEED_DECREMENT)
        {
            currentSpeed -= SPEED_DECREMENT;
            if(hat.gameObject.GetComponent<Rigidbody2D>().drag > 0) hat.gameObject.GetComponent<Rigidbody2D>().drag -= 0.1f;
            Debug.Log("Speed Up");
        }
        if (hat.name == "Doctor Hat" && numLives < 9)
        {
            numLives++;
            saveManager.saveGlob.totalDoctorsHats++;
            achievementsAdd("First Aid", "Heal yourself");
            lives[numLives - 1].gameObject.SetActive(true);
        }
    }

    public void AnvilHit()
    {
        this.GetComponent<AudioSource>().Stop();
        achievementsAdd("Ouch", "Get hit with an anvil");
        Debug.Log("Anvil");
        saveManager.saveGlob.totalAnvilsFallen++;
        Debug.Log("Game Over");
        gamePlaying = false;
        EndGame(); 
    }

    public void HatMissed()
    {
        lives[numLives - 1].gameObject.SetActive(false);
        if (numLives > 0)
        {
            //Debug.Log("Missed Hat");
            Debug.Log("Lives Left: " + numLives);
            numLives--;
        }
        if (numLives == 0)
        {
            Debug.Log("Game Over");
            gamePlaying = false;
            EndGame();
        }
    }


    void EndGame()
    {
        enabled = false;
        playerInstance.gameObject.GetComponent<MouseMove2D>().enabled = false;
        GameObject[] hatsRemaining = GameObject.FindGameObjectsWithTag("Hat");
        foreach (GameObject c in hatsRemaining) Destroy(c.gameObject);
        StopAllCoroutines();
        TestForAchievements();
        Leaderboard();
        StartCoroutine("EndGameScroll");
    }

    IEnumerator EndGameScroll()
    {
        foreach (Image life in lives) life.gameObject.SetActive(false);
        life.gameObject.SetActive(false);
        float t = 0.0f;
        Vector3 startingPos = camera1.transform.position;
        Vector3 target = new Vector3(playerInstance.transform.position.x, playerInstance.transform.position.y + 3, camera1.transform.position.z);
        float transitionDuration = 2f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            camera1.transform.position = Vector3.Lerp(startingPos, target, t);
            yield return 0;
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Title");
    }

    void Leaderboard()
    {
        Debug.Log("Leader: " + leaderboard.Count);
        Leaderboard currentScore = new Leaderboard(DateTime.Now.ToString("MMMM dd, hh:mm "), numHatsCollected);
        if (leaderboard.Count == 0) leaderboard.Add(currentScore);
        else
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Leaderboard temp = leaderboard[i];
                    if (currentScore > temp)
                    {
                        leaderboard.Insert(i, currentScore);
                        break;
                    }
                }
                catch { break;  }
            }
            if(leaderboard.Count > 5) leaderboard.RemoveRange(5, leaderboard.Count - 5);
        }
        Debug.Log("Leader: " + leaderboard.Count);
        saveManager.saveGlob.leaderboard = leaderboard;
        Debug.Log("Leader: " + leaderboard.Count);
        foreach(Leaderboard l in leaderboard)
        {
            Debug.Log (l.name +" : "+ l.score);
        }
        saveManager.SaveDataToDisk();
    }

    void TestForAchievements() //creates and adds achievements to list
    {
        if (numLives == 9) achievementsAdd("Nine Lives", "Die with all nine lives remaining");

        if (hatsCollected.Contains("Beret")) achievementsAdd("Where zey make ze toast", "Catch the Beret");
        if (hatsCollected.Contains("Fez")) achievementsAdd("Fezzes are cool", "Catch the Fez");
        if (hatsCollected.Contains("Ushanka")) achievementsAdd("For Mother Russia", "Catch the Ushanka");
        if (hatsCollected.Contains("Soda Hat")) achievementsAdd("Game Day", "Catch the Soda Hat");
        if (hatsCollected.Contains("Graduation Cap")) achievementsAdd("Freedom", "Catch the Graduation Cap");

        if (saveManager.saveGlob.totalAnvilsFallen > 10) achievementsAdd("Anvil Magnet", "Get hit with 10 anvils");
        if (saveManager.saveGlob.totalAnvilsFallen > 50) achievementsAdd("Masochist", "Get hit with 50 anvils");
        if (saveManager.saveGlob.totalDoctorsHats > 10) achievementsAdd("Medic", "Heal yourself 10 times");
        if (saveManager.saveGlob.totalDoctorsHats > 25) achievementsAdd("Doctor", "Heal yourself 25 times");

        if (numHatsCollected > 10) achievementsAdd("10 Stack", "Catch 10 hats in a game");
        if (numHatsCollected > 25) achievementsAdd("25 Stack", "Catch 25 hats in a game");
        if (numHatsCollected > 50) achievementsAdd("50 Stack", "Catch 50 hats in a game");
        if (numHatsCollected > 100) achievementsAdd("100 Stack", "Catch 100 hats in a game");
        if (numHatsCollected > 150) achievementsAdd("150 Stack", "Catch 150 hats in a game");


        saveManager.saveGlob.completedAchievements = achievements;
    }

    public bool achievementsAdd(string n, string s)
    {
        Achievements temp = new Achievements(n,s);
        foreach(Achievements a in achievements)
        {
            if (temp == a) return false;
        }
        achievements.Add(temp);
        return true;
    }

    public static void TogglePause()
    {
        if (gm.paused)
        {
            Debug.Log("unpause");
            gm.paused = false;
            Time.timeScale = 1;
            gm.playerInstance.GetComponent<MouseMove2D>().enabled = true;
        }
        else
        {
            gm.paused = true;
            Time.timeScale = 0;
            gm.playerInstance.GetComponent<MouseMove2D>().enabled = false;
        }
    }
    public Camera GetCamera()
    {
        return camera1;
    }
}
