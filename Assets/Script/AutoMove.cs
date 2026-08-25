using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AutoMove : MonoBehaviour
{
    public bool isRight;
    private Rigidbody2D rb;
    public float speed = 5f;
    public float max = 10f;
    private float current;
    private SpriteRenderer sr;
    private Vector2 oldPos;
    private Vector2 newPos;
    public Vector2 distance;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        current = rb.position.x;
        sr = GetComponent<SpriteRenderer>();
        // current - max <= x <= max + current
        oldPos = rb.transform.position;
    }

    private void FixedUpdate()
    {
        //tinh khoang cach tu vi tri cu toi vi tri moi
        //de biet ufo da di chuyen duoc bao nhieu
        newPos = rb.transform.position;
        distance = newPos - oldPos;
        //Debug.Log("distance: " + distance);

        oldPos = newPos;
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
