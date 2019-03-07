using UnityEngine;
using System.Collections;

public class MouseMove2D : MonoBehaviour
{

    private Vector3 mousePosition;
    public float moveSpeed = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        mousePosition = new Vector3(Input.mousePosition.x,0,0);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);        
    }
}