using System.Collections;
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
    private List<Achievements> achievements;
    private bool gamePlaying;
    private bool paused;
    private List<Image> lives;
    #endregion

    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        gm = this;
        saveManager = new SaveManager();
        achievements = saveManager.saveGlob.completedAchievements;
        numLives = 9;
        currentSpeed = SPEED_INIT;
        StartCoroutine("StartGame");
        hatStreak = 0;
        lastHat = null;
        gamePlaying = true;
        paused = false;
        lives = PauseButton.GetLives();
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
        numHatsCollected++;
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
        }
        else
        {
            lastHat = hat;
            hatStreak = 1;
        }
        if (hat.name == "Doctor Hat" && numLives < 9)
        {
            numLives++;
            lives[numLives].gameObject.SetActive(true);
        }
    }

    public void AnvilHit()
    {
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
        NewHighScore();
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

    void NewHighScore()
    {
        if (numHatsCollected > saveManager.saveGlob.highscore) saveManager.saveGlob.highscore = numHatsCollected;
    }

    void TestForAchievements() //creates and adds achievements to list
    {

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
