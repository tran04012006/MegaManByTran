using System;
using UnityEngine;
public class Health : MonoBehaviour
{
    //array thanh mau
    public Sprite[] healthPrites;
    private SpriteRenderer sr;
    private int hp;
    public PlayerController pc;
    private int currentHPSprite = 0;
    public int max = 17;
    private Hurt h;

    private void Start()
    {
        hp = max;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = healthPrites[currentHPSprite]; //ban dau thi sprite la full mau
        pc.onHpChange += changeHP;
        h = GetComponent<Hurt>();
        //su kien 1: thay doi so hp + thay doi sprite

    }
    //dang ki su kien action tang giam mau tu GameManager
    
    private void changeHP(int hp)
    {
        h.getHurt();
        this.hp = this.hp + hp;
        if (this.hp <= 0)
            this.hp = 0;
        if (this.hp >= max)
            this.hp = max;
        // neu giam hp thi truyen vao -1
        //neu tang hp thi truyen vao 1
        //thay doi sprite
        currentHPSprite++;
        if (currentHPSprite >= max)
            currentHPSprite = max;
        sr.sprite = healthPrites[currentHPSprite]; 
    }
    
}
