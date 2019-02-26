using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Hats
{
    public string type;
    public Sprite image;
    public float probability;
}
public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    [SerializeField]
    public List<Hats> hatTypes = new List<Hats>();
    public GameObject anvilPrefab;
    public float anvilProbability;
    private const float SPEED_INIT = 5f; //initial time between drops
    private const float MAX_SPEED = 0.1f; //max hat drop speed
    private float currentSpeed; //stores current time between drops
    private int numHatsCollected;
    private GameObject playerInstance;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartGame");
    }

    IEnumerator StartGame()
    {
        //spawn player
        playerInstance = Instantiate(playerPrefab) as GameObject;
        playerInstance.transform.position = new Vector3();
        //Countdown
        for (int i = 3; i >= 1; i--)
        {
            Debug.Log(i); //insert countdown here
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Start!"); //insert Start! here
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
