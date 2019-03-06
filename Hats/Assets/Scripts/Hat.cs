using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hat : MonoBehaviour
{
    public Sprite image;
    public float skipProbability;
    public float height;

    private void Start()
    {
        height = gameObject.GetComponent<Renderer>().bounds.size.y;
    }
}
