﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gmInstance;
    private float height;
    private void Start()
    {
        tag = "Player";
        gmInstance = GameManager.gm;
        height = GetComponent<Collider2D>().offset.y;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Anvil")
        {
            Destroy(other.gameObject.GetComponent<Hat>());
            if (PlayerPrefs.GetInt("Sound") == 1) { other.gameObject.GetComponent<AudioSource>().Play(0); }
            gmInstance.AnvilHit();
        }

        else
        {
            if (PlayerPrefs.GetInt("Sound") == 1) other.gameObject.GetComponent<AudioSource>().Play(0);
            other.transform.parent = this.transform;
            height += other.transform.gameObject.GetComponent<Hat>().height;
            GetComponent<BoxCollider2D>().offset = new Vector2(0, height);
            Destroy(other.gameObject.GetComponent<Rigidbody2D>());
            Destroy(other.gameObject.GetComponent<BoxCollider2D>());
            other.gameObject.tag = "Player";
            other.transform.localPosition = new Vector3(0, height,5);
            other.transform.localRotation = Quaternion.identity;
            Hat temp = other.gameObject.GetComponent<Hat>();
            gmInstance.HatCollected(temp);
            other.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = gmInstance.numHatsCollected;
            Destroy(other.gameObject.GetComponent<Hat>());

        }

    }
}