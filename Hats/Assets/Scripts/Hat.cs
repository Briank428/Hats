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
        BoxCollider2D c = gameObject.GetComponent<BoxCollider2D>();
        height = c.bounds.size.y * transform.localScale.y;
        gmInstance = GameManager.gm;
        name = name.Remove(name.IndexOf("(Clone)"));
    }

    private void Update()
    {
        if (this.tag == "Hat" && transform.position.y < Camera.main.transform.position.y - 10)
        {
            gmInstance.HatMissed();
            Destroy(this.gameObject);
        }
    }

}
