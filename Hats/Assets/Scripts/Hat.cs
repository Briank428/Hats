using System.Collections;
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
        height = gameObject.GetComponent<BoxCollider2D>().bounds.size.y;
        this.tag = "Hat";
        if (this.name == "Anvil")
        {
            rb.mass = 2;
            this.tag = "Anvil";
        }
    }

    private void Update()
    {
        if (transform.localPosition.y < -5)
        {
            gmInstance.HatMissed();
            Destroy(this);
        }
    }
}
