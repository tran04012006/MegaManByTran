using System;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;

    public float lifeTime = 3f;

    private bool isRight = false;

    private SpriteRenderer sr;

    public float damage;
    public int score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //khi sinh ra la bi destroy luon
        Destroy(gameObject, lifeTime);
        sr = GetComponent<SpriteRenderer>();
    }

    public void setDirection(bool isRight)
    {
        //cai nay se quyet dinh huong cua nhan vat
        this.isRight = isRight;
        
    }

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("dung enemy");
            GameManager.Instance.addScore(score);
            // hieu ung dung nhan vat
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            //hp cua player bi tru diem
            GameManager.Instance.onHpChange.Invoke(-1);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
        //cau lenh di chuyen nhan vat kha hay
        Debug.Log(isRight);
        if (isRight == true) //di ve ben phai
        {
            sr.flipX = true;
            //dua dan di sang ben phai
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (isRight == false)
        {
            sr.flipX = false;
            //dua dan di sang ben trai
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        
    }
}
