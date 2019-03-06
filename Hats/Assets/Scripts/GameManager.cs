using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<Hat> hatTypes = new List<Hat>();
    private const float SPEED_INIT = 5.0f; //initial time between drops
    private const float MAX_SPEED = 0.1f; //max hat drop speed
    private const float SPEED_DECREMENT = 0.1f;
    private float currentSpeed; //stores current time between drops
    public Player playerPrefab;
    private Player playerInstance;
    private int numHatsCollected;
    private int numLives;
    private GameManager gmInstance;
    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        numLives = 9;
        currentSpeed = SPEED_INIT;
        //StartCoroutine("StartGame");
        gmInstance = this;
    }

    IEnumerator StartGame()
    {
        //spawn player
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.transform.position = Vector3.zero;
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
        temp2.transform.position = new Vector2(random.Next(-5,5),5);
    }

    public void HatCollected()
    {
        numHatsCollected++;
        if (numHatsCollected % 10 == 0 && currentSpeed > MAX_SPEED) currentSpeed -= SPEED_DECREMENT;
    }

    public void AnvilHit()
    {
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
