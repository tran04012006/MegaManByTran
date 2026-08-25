using System;
using System.Collections;
using UnityEngine;
public class Shooting : MonoBehaviour
{
    private bool isRight = false;
    public GameObject bullet;
    private Transform point;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void shoot()
    {
        
            //neu bi ban thi se ban nguoc lai
            // lay 2 con cua gameobject ra
            if (sr.flipX == false)
            {
                isRight = false;
                Debug.Log("dang xoay sang trai");
                //xoay sang ben trai
                point = gameObject.transform.GetChild(0);
            }
            if (sr.flipX == true)
            {
                isRight = true;
                Debug.Log("dang xoay sang phai");
                //xoay sang ben phai
                point = gameObject.transform.GetChild(1);
            }
        
                //tao ra vien dan
                GameObject newBullet = Instantiate(bullet, point.position, point.rotation);
                SoundManager.Instance.currentSound = SoundManager.SoundID.EnemyShoot;
                //thay doi huong cua vien dan
                EnemyBulletController bc = newBullet.gameObject.GetComponent<EnemyBulletController>();
                bc.setDirection(isRight);
                //SoundManager.Instance.currentSound = SoundManager.SoundID.EnemyShoot;
        
    }

}
