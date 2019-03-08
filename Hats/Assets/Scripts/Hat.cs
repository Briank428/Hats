﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hat : MonoBehaviour
{
    public float skipProbability;
    public float height;
    public GameManager gmInstance;

    private void Start()
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.mass = 1;
        BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
        height = c.bounds.size.y;
        Debug.Log("Hat Height: " + height);
        gmInstance = GameManager.gm;
    }

    private void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - 10)
        {
            gmInstance.HatMissed();
            Destroy(this.gameObject);
        }
    }

}
