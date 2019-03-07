using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager gmInstance;

    #region public vars
    [SerializeField]
    public List<Hat> hatTypes = new List<Hat>();
    public Player playerPrefab;
    #endregion

    #region consts
    private const float SPEED_INIT = 2.0f; //initial time between drops
    private const float MAX_SPEED = 0.1f; //max hat drop speed
    private const float SPEED_DECREMENT = 0.1f;
    #endregion

    #region private vars
    private float currentSpeed; //stores current time between drops
    private Player playerInstance;
    private int numHatsCollected;
    private int numLives;
    private Hat lastHat;
    private int hatStreak;
    #endregion

    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        numLives = 9;
        currentSpeed = SPEED_INIT;
        StartCoroutine("StartGame");
        gmInstance = this;
        hatStreak = 0;
        lastHat = null;
    }

    IEnumerator StartGame()
    {
        //spawn player
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.position = new Vector2(0,-5);
        playerInstance.gmInstance = gmInstance;

        //Countdown
        for (int i = 3; i >= 1; i--)
        {
            Debug.Log(i); //insert countdown here
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Start!"); //insert Start! here

        while (numLives != 0)
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
        } while (random.Next(0,1) < temp.skipProbability);

        //spawn hat

        Hat temp2 = Instantiate(temp) as Hat;
        temp2.gmInstance = gmInstance;
        temp2.transform.position = new Vector2(random.Next(-5,5),5);
    }

    public void HatCollected(Hat hat)
    {
        numHatsCollected++;
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
        Achievements.HatCollected(lastHat, hatStreak);
    }

    public void AnvilHit()
    {
        Achievements.AnvilHitLives(numLives);
        numLives = 0;
    }
    public void HatMissed()
    {
        numLives--;
    }
    // Update is called once per frame
    void Update()
    {
        if (numLives == 0) StopAllCoroutines();
    }
}
