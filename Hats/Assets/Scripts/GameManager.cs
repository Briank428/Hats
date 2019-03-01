using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<Hat> hatTypes = new List<Hat>();
    public GameObject anvilPrefab;
    public const float ANVIL_PROBABILITY = 0.3f;
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
            if (random.Next(0,1) < ANVIL_PROBABILITY) SpawnAnvil();
            else { SpawnHat(); }
        }
    }
    void SpawnHat()
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

    void SpawnAnvil()
    {
        GameObject temp = Instantiate(anvilPrefab) as GameObject;
        temp.transform.position = new Vector2(random.Next(-5, 5), 5);
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
