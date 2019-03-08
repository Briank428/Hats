﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    #endregion

    #region consts
    private const float SPEED_INIT = 1.0f; //initial time between drops
    private const float MAX_SPEED = 0.1f; //max hat drop speed
    private const float SPEED_DECREMENT = 0.2f;
    #endregion

    #region private vars
    private SaveManager saveManager;
    private float currentSpeed; //stores current time between drops
    private Player playerInstance;
    [SerializeField]
    private int numHatsCollected;
    private int numLives;
    private Hat lastHat;
    private int hatStreak;
    private Hashtable achievements;
    private List<Leaderboard> leaderboard;
    private bool gamePlaying;
    private bool paused;
    private List<Image> lives;
    private List<string> hatsCollected;
    #endregion

    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        counter.text = "0";
        gm = this;
        saveManager = new SaveManager();
        achievements = saveManager.saveGlob.completedAchievements;
        leaderboard = saveManager.saveGlob.leaderboard;
        numLives = 9;
        currentSpeed = SPEED_INIT;
        StartCoroutine("StartGame");
        hatStreak = 0;
        lastHat = null;
        gamePlaying = true;
        paused = false;
        lives = PauseButton.GetLives();
        hatsCollected = new List<string>();
    }

    IEnumerator StartGame()
    {
        //spawn player
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.position = new Vector2(0, -5);

        //Countdown
        for (int i = 3; i >= 1; i--)
        {
            Debug.Log(i); //insert countdown here
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Start!"); //insert Start! here

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
        } while (random.Next(0, 1) < temp.skipProbability);

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
        spawnPoint.position += new Vector3(0, hat.height, 0);
        camera1.transform.position += new Vector3(0, hat.height, 0);
        if (numHatsCollected % 10 == 0 && currentSpeed > MAX_SPEED + SPEED_DECREMENT)
        {
            currentSpeed -= SPEED_DECREMENT;
            Debug.Log("Speed Up");
        }
        if (lastHat == hat)
        {
            hatStreak++;
            if (hatStreak == 7 && hat.name == "Top Hat") achievements.Add("Four score and 7 hats ago", "Catch 7 Top Hats in a row");
        }
        else
        {
            lastHat = hat;
            hatStreak = 1;
        }
        if (hat.name == "Doctor Hat" && numLives < 9)
        {
            numLives++;
            saveManager.saveGlob.totalDoctorsHats++;
            achievements.Add("First Aid", "Heal yourself");
            lives[numLives].gameObject.SetActive(true);
        }
    }

    public void AnvilHit()
    {
        achievements.Add("Ouch", "Get hit with an anvil");
        Debug.Log("Anvil");
        saveManager.saveGlob.totalAnvilsFallen++;
        Debug.Log("Game Over");
        gamePlaying = false;
        EndGame(); ;
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
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void EndGame()
    {
        enabled = false;
        playerInstance.gameObject.GetComponent<MouseMove2D>().enabled = false;
        GameObject[] hatsRemaining = GameObject.FindGameObjectsWithTag("Hat");
        foreach (GameObject c in hatsRemaining) Destroy(c.gameObject);
        StopAllCoroutines();
        StartCoroutine("EndGameScroll");
        TestForAchievements();
        Leaderboard();
        saveManager.saveDataToDisk();
        Debug.Log(saveManager.saveGlob.highscore);
    }

    IEnumerator EndGameScroll()
    {
        float t = 0.0f;
        Vector3 startingPos = camera1.transform.position;
        Vector3 target = new Vector3(playerInstance.transform.position.x, playerInstance.transform.position.y, camera1.transform.position.z);
        float transitionDuration = 2f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            camera1.transform.position = Vector3.Lerp(startingPos, target, t);
            yield return 0;
        }
        SceneManager.LoadScene("Title");
    }

    void Leaderboard()
    {
        if (numHatsCollected > saveManager.saveGlob.highscore) saveManager.saveGlob.highscore = numHatsCollected;
        try
        {
            for (int i = 0; i < leaderboard.Count; i++)
            {
                Leaderboard l = leaderboard[i];
                if (l == null || numHatsCollected > l.score) leaderboard.Insert(i, new Leaderboard(PlayerPrefs.GetString("Name"), numHatsCollected));
            }
            leaderboard.RemoveAt(leaderboard.Count - 1);
        }
        catch
        {
            leaderboard.Add(new Leaderboard(PlayerPrefs.GetString("Name"), numHatsCollected));
        }
        saveManager.saveGlob.leaderboard = leaderboard;
    }

    void TestForAchievements() //creates and adds achievements to list
    {
        if (numLives == 9) achievements.Add("Nine Lives", "Die with all nine lives remaining");

        if (hatsCollected.Contains("Beret")) achievements.Add("Where zey make ze toast", "Catch the Beret");
        if (hatsCollected.Contains("Fez")) achievements.Add("Fezzes are cool", "Catch the Fez");
        if (hatsCollected.Contains("Ushanka")) achievements.Add("For Mother Russia", "Catch the Ushanka");
        if (hatsCollected.Contains("Soda Hat")) achievements.Add("Game Day", "Catch the Soda Hat");
        if (hatsCollected.Contains("Graduation Cap")) achievements.Add("Graduation Day", "Catch the Graduation Cap");

        if (saveManager.saveGlob.totalAnvilsFallen > 10) achievements.Add("Anvil Magnet", "Get hit with 10 anvils");
        if (saveManager.saveGlob.totalAnvilsFallen > 50) achievements.Add("Masochist", "Get hit with 50 anvils");
        if (saveManager.saveGlob.totalDoctorsHats > 10) achievements.Add("Medic", "Heal yourself 10 times");
        if (saveManager.saveGlob.totalDoctorsHats > 25) achievements.Add("Doctor", "Heal yourself 25 times");

        if (numHatsCollected > 10) achievements.Add("10 Stack", "Catch 10 hats in a game");
        if (numHatsCollected > 25) achievements.Add("25 Stack", "Catch 25 hats in a game");
        if (numHatsCollected > 50) achievements.Add("50 Stack", "Catch 50 hats in a game");
        if (numHatsCollected > 100) achievements.Add("100 Stack", "Catch 100 hats in a game");

        saveManager.saveGlob.completedAchievements = achievements;
    }

    public static void TogglePause()
    {
        if (gm.paused)
        {
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

}
