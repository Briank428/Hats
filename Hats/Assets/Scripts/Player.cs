using System.Collections;
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
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Anvil")
        {
            if (MusicFX.sound) { other.gameObject.GetComponent<AudioSource>().Play(0); Debug.Log("Anvil"); }
            if (MusicFX.music) { this.GetComponent<AudioSource>().Stop(); }
            gmInstance.AnvilHit();
        }

        else
        {
            if (MusicFX.sound) other.gameObject.GetComponent<AudioSource>().Play(0);
            other.transform.parent = this.transform;
            height += other.transform.gameObject.GetComponent<Hat>().height;
            GetComponent<BoxCollider2D>().offset = new Vector2(0, height);
            Destroy(other.gameObject.GetComponent<Rigidbody2D>());
            Destroy(other.gameObject.GetComponent<BoxCollider2D>());
            other.gameObject.tag = "Player";
            other.transform.localPosition = new Vector2(0, height);
            other.transform.localRotation = Quaternion.identity;
            Hat temp = other.gameObject.GetComponent<Hat>();
            gmInstance.HatCollected(temp);
            other.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = gmInstance.numHatsCollected;
            Destroy(other.gameObject.GetComponent<Hat>());

        }

    }
}