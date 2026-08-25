using System;
using UnityEngine;
public class Health : MonoBehaviour
{
    //array thanh mau
    public Sprite[] healthPrites;
    private SpriteRenderer sr;
    private float hp;
    public PlayerController pc;

    public float currentHPSprite = 0;
    public int max = 17;
    private Hurt h;
    private bool playerDie = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = healthPrites[(int)currentHPSprite]; //ban dau thi sprite la full mau
    }

    private void Start()
    {
        hp = max;
        h = GetComponent<Hurt>();
        //su kien 1: thay doi so hp + thay doi sprite

    }

    private void OnEnable()
    {
        pc.onHpChange += changeHP;
    }

    private void OnDisable()
    {
        pc.onHpChange -= changeHP;
    }

    public void changeHP(float hp)
    {
        this.hp = this.hp + hp;
        if (this.hp <= 0)
            this.hp = 0;
        if (this.hp >= max)
            this.hp = max;
        GameManager.Instance.hp = this.hp;
        // neu giam hp thi truyen vao -1
        //neu tang hp thi truyen vao 1
        //thay doi sprite
        currentHPSprite += hp;
        if (currentHPSprite >= max)
        {
            //luc nay thi player da chet
            currentHPSprite = max - 1;
            if (playerDie == false)
            {
                pc.isDead(); //bao la player da chet
                playerDie = true;
            }
        }
        if (currentHPSprite <= 0)
        {
            currentHPSprite = 0;
        }
        sr.sprite = healthPrites[(int)currentHPSprite]; 
        Debug.Log("currentHPSprite: " + currentHPSprite);
    }
    
}
