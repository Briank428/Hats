using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameManager gmInstance;

    #region public vars
    [SerializeField]
    public List<Hat> hatTypes = new List<Hat>();
    public Player playerPrefab;
    public Transform spawnPoint;
    #endregion

    #region consts
    private const float SPEED_INIT = 0.5f; //initial time between drops
    private const float MAX_SPEED = 0.5f; //max hat drop speed
    private const float SPEED_DECREMENT = 0.1f;
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
    #endregion

    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        saveManager = new SaveManager();
        achievements = saveManager.saveGlob.completedAchievements;
        numLives = 9;
        currentSpeed = SPEED_INIT;
        StartCoroutine("StartGame");
        gmInstance = this;
        hatStreak = 0;
        lastHat = null;
        gamePlaying = true;
        paused = false;
    }

    IEnumerator StartGame()
    {
        //spawn player
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.position = new Vector2(0, -5);
        playerInstance.gmInstance = this;

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
        temp2.gmInstance = gmInstance;
        float width = random.Next(-5, 5);
        width = 0;
        temp2.transform.position = spawnPoint.position;
    }

    public void HatCollected(Hat hat)
    {
        Debug.Log("Hat");
        numHatsCollected++;
        spawnPoint.position += new Vector3(0, hat.height, 0);
        Camera.main.transform.position += new Vector3(0, hat.height, 0);
        if (numHatsCollected % 10 == 0 && currentSpeed > MAX_SPEED) currentSpeed -= SPEED_DECREMENT;
        if (lastHat == hat)
        {
            hatStreak++;
        }
        else
        {
            lastHat = hat;
            hatStreak = 1;
        }
        if (hat.name == "Doctor Hat") numLives++;
    }

    public void AnvilHit()
    {
        Debug.Log("Anvil");
        saveManager.saveGlob.totalAnvilsFallen++;
        gamePlaying = false;
    }
    public void HatMissed()
    {
        if (numLives > 0)
        {
            //Debug.Log("Missed Hat");
            //Debug.Log("Lives Left: " + numLives);
            numLives--;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (numLives == 0 || !gamePlaying)
        {
            Debug.Log("Game Over");
            gamePlaying = false;
            EndGame();

        }
    }

    void EndGame()
    {
        StopAllCoroutines();
        StartCoroutine("EndGameScroll");
        TestForAchievements();
        NewHighScore();
        saveManager.saveDataToDisk();
        Debug.Log(saveManager.saveGlob.highscore);
        SceneManager.LoadScene("Title");
    }

    IEnumerator EndGameScroll()
    {
        float t = 0.0f;
        Vector3 startingPos = Camera.main.transform.position;
        Transform target = playerInstance.transform;
        float transitionDuration = numHatsCollected * .5f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            transform.position = Vector3.Lerp(startingPos, target.position, t);
            yield return 0;
        }
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

    }

}
