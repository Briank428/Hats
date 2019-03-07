using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gmInstance;
    private float height;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision");
        if (other.transform.tag == "Anvil")
        {
            gmInstance.AnvilHit();
        }
  
        else
        {
            other.transform.parent = this.transform;

            Transform temp1 = other.transform;
            GameObject temp2 = temp1.gameObject;
            Hat temp3 = temp2.GetComponent<Hat>();
            float temp4 = temp3.height;

            height += other.transform.gameObject.GetComponent<Hat>().height;
            GetComponent<BoxCollider2D>().offset = new Vector2(0,other.transform.gameObject.GetComponent<Hat>().height);
            Destroy(other.gameObject.GetComponent<Rigidbody2D>());
            Destroy(other.gameObject.GetComponent<BoxCollider2D>());
            other.transform.localPosition = new Vector2(0,height);
            Hat temp = other.gameObject.GetComponent<Hat>();
            gmInstance.HatCollected(temp);
        }

    } 
}