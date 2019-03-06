using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hat : MonoBehaviour
{
    public Sprite image;
    public float skipProbability;
    public float height;
    public GameManager gmInstance;

    private void Start()
    {
        //height = gameObject.GetComponent<Renderer>().bounds.size.y;
        Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
        rb.mass = 5;
        if (this.name == "Anvil") rb.mass = 10;
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
