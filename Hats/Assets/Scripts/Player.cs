using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gmInstance;
    private string playerName;
    private float height;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "anvil") {
            other.transform.parent = this.transform;
            height += other.transform.gameObject.GetComponent<Hat>().height;
            other.transform.localPosition = new Vector2(0,height);

        }
    }
}
