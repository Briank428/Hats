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
    public Text counter;
    public Text life;
    public List<Image> lives;
    public int numHatsCollected;

    #endregion

    #region consts
    private const float SPEED_INIT = 0.8f; //initial time between drops
    private const float MAX_SPEED = 0.20f; //max hat drop speed
    private const float SPEED_DECREMENT = 0.05f;
    #endregion

    #region private vars
    private SaveManager saveManager;
    private float currentSpeed; //stores current time between drops
    private Player playerInstance;
    [SerializeField]
    private int numLives;
    private Hat lastHat;
    private int hatStreak;
    private List<Achievements> achievements;
    private List<Leaderboard> leaderboard;
    private bool gamePlaying;
    private bool paused;
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
        foreach (Image i in lives) i.gameObject.SetActive(true);
        hatsCollected = new List<string>();
    }

    IEnumerator StartGame()
    {
        //spawn player
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.position = new Vector2(0, -10);
        if (MusicFX.music) playerInstance.gameObject.GetComponent<AudioSource>().Play();

        //Countdown
        for (int i = 3; i >= 1; i--)
        {
            Debug.Log(i); //insert countdown here
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Start!"); //insert Start! here

        if (PlayerPrefs.GetString("Name") == "USE_4_TEST")
        {
            StartCoroutine(SpawnAllHats());
        }
        else
        {
            while (gamePlaying)
            {
                yield return new WaitForSeconds(currentSpeed);
                SpawnHat();
            }
        }
    }

    IEnumerator SpawnAllHats()
    {
        foreach(Hat h in hatTypes)
        {
            numLives = 9;
            Hat temp2 = Instantiate(h) as Hat;
            float width = 0;
            spawnPoint.position = new Vector2(width, spawnPoint.position.y);
            temp2.transform.position = spawnPoint.position;
            yield return new WaitForSeconds(.5f);
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
            if (hatStreak == 7 && hat.name == "Top Hat") achievementsAdd("Four score and 7 hats ago", "Catch 7 Top Hats in a row");
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
            achievementsAdd("First Aid", "Heal yourself");
            lives[numLives - 1].gameObject.SetActive(true);
        }
    }

    public void AnvilHit()
    {
        achievementsAdd("Ouch", "Get hit with an anvil");
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
    }

    IEnumerator EndGameScroll()
    {
        foreach (Image life in lives) life.gameObject.SetActive(false);
        life.gameObject.SetActive(false);
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Title");
    }

    void Leaderboard()
    {
        if (leaderboard.Count == 0) leaderboard.Add(new Leaderboard(PlayerPrefs.GetString("Name"), numHatsCollected));
        if (leaderboard.Count == 5) {
            bool inserted = false;
            for (int i = 0; i < leaderboard.Count; i++)
            {
                Leaderboard place = leaderboard[i];
                if (place.GetScore() < numHatsCollected) { leaderboard.Insert(i, new Leaderboard(PlayerPrefs.GetString("Name"), numHatsCollected)); inserted = true;  }
            }
            if (!inserted) leaderboard.Add(new Leaderboard(PlayerPrefs.GetString("Name"), numHatsCollected));
        }
        else
        {
            bool inserted = false;
            for (int i = 0; i < leaderboard.Count; i++)
            {
                Leaderboard place = leaderboard[i];
                if (place.GetScore() < numHatsCollected) { leaderboard.Insert(i, new Leaderboard(PlayerPrefs.GetString("Name"), numHatsCollected)); inserted = true;  }
            }
            if (inserted) leaderboard.RemoveAt(5);
        }
        Debug.Log("Leader: " + leaderboard.Count);
        saveManager.saveGlob.leaderboard = leaderboard;
        Debug.Log("Leader: " + leaderboard.Count);
    }

    void TestForAchievements() //creates and adds achievements to list
    {
        if (numLives == 9) achievementsAdd("Nine Lives", "Die with all nine lives remaining");

        if (hatsCollected.Contains("Beret")) achievementsAdd("Where zey make ze toast", "Catch the Beret");
        if (hatsCollected.Contains("Fez")) achievementsAdd("Fezzes are cool", "Catch the Fez");
        if (hatsCollected.Contains("Ushanka")) achievementsAdd("For Mother Russia", "Catch the Ushanka");
        if (hatsCollected.Contains("Soda Hat")) achievementsAdd("Game Day", "Catch the Soda Hat");
        if (hatsCollected.Contains("Graduation Cap")) achievementsAdd("Graduation Day", "Catch the Graduation Cap");

        if (saveManager.saveGlob.totalAnvilsFallen > 10) achievementsAdd("Anvil Magnet", "Get hit with 10 anvils");
        if (saveManager.saveGlob.totalAnvilsFallen > 50) achievementsAdd("Masochist", "Get hit with 50 anvils");
        if (saveManager.saveGlob.totalDoctorsHats > 10) achievementsAdd("Medic", "Heal yourself 10 times");
        if (saveManager.saveGlob.totalDoctorsHats > 25) achievementsAdd("Doctor", "Heal yourself 25 times");

        if (numHatsCollected > 10) achievementsAdd("10 Stack", "Catch 10 hats in a game");
        if (numHatsCollected > 25) achievementsAdd("25 Stack", "Catch 25 hats in a game");
        if (numHatsCollected > 50) achievementsAdd("50 Stack", "Catch 50 hats in a game");
        if (numHatsCollected > 100) achievementsAdd("100 Stack", "Catch 100 hats in a game");

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
