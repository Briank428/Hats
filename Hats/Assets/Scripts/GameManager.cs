using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Hats
{
    float probability;
    string type;
    Sprite image;
}
public class GameManager : MonoBehaviour
{
    public List<Hats> hatTypes;
    public GameObject anvilPrefab;
    public float anvilProbability;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartGame");
    }

    IEnumerator StartGame()
    {
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
