using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AutoMove : MonoBehaviour
{
    public bool isRight = false;
    private Rigidbody2D rb;
    public float speed = 5f;
    public float max = 10f;
    private float current;
    private SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        current = rb.position.x;
        sr = GetComponent<SpriteRenderer>();
        // current - max <= x <= max + current
    }

    private void FixedUpdate()
    {
        if (isRight == false)
        {
            sr.flipX = false;
            // di sang trai
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            if (rb.position.x < current - max)
            {
                isRight = true;
            }
        }
        
        else
        {
            sr.flipX = true;

            //di sang phai
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            if (rb.position.x > current + max)
                isRight = false;
        }
        
    }
}
