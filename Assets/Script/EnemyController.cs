using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected SpriteRenderer sr;
    private GameManager gm;
    private Hurt h;
    public Action<float> HPEnemyChange;
    public float score;
    private const float damageOfPlayer = 5;
    public GameObject particleEffect;
    public EnemyItem enemyData;
    private float damage;
    public float hp;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        h = GetComponent<Hurt>();
        damage = enemyData.damage;
        hp = enemyData.hp;
    }
    

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        //bi tan cong
        if (other.gameObject.CompareTag("Bullet"))
        {
            h.getHurt();
            TakeDamage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //enemy dung vao player
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.TakeDamage(damage);
        }
    }
    
    public void TakeDamage()
    {
        //thay doi bar mau
        hp = hp - damageOfPlayer;
        Debug.Log("hp: !!!!!!!!!!!!!!!!!!!!!!!!! " + hp);
        HPEnemyChange?.Invoke(hp);
        //HPbarController
        if (hp <= 0)
        {
            //enemy chet thi cong 10
            IsDead();
            ScoreUpdate.Instance.UpdateScore(10);
        }
    }

    public virtual void IsDead()
    {
        Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
