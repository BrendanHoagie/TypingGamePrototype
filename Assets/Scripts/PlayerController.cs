using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    float xSpeed, ySpeed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xSpeed = Input.GetAxisRaw("HorizontalArrows") * moveSpeed;
        ySpeed = Input.GetAxisRaw("VerticalArrows") * moveSpeed;
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }
}
