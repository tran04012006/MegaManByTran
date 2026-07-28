using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private GameManager gm;
    private Hurt h;

    public float score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        h = GetComponent<Hurt>();
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        //bi tan cong
        if (other.gameObject.CompareTag("Bullet"))
        {
            h.getHurt();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
