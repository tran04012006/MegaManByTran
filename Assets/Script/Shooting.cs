using System;
using System.Collections;
using UnityEngine;
public class Shooting : MonoBehaviour
{
    private bool isRight = false;
    public GameObject bullet;
    private Transform point;
    
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("da bi trung dan");
            //neu bi ban thi se ban nguoc lai
                // lay 2 con cua gameobject ra
                if (isRight == false)
                {
                    //xoay sang ben trai
                    point = gameObject.transform.GetChild(0);
                }
                else
                {
                    //xoay sang ben phai
                    point = gameObject.transform.GetChild(1);
                }
                //tao ra vien dan
                GameObject newBullet = Instantiate(bullet, point.position, point.rotation);
                //thay doi huong cua vien dan
                BulletController bc = newBullet.gameObject.GetComponent<BulletController>();
                bc.setDirection(isRight);
            
        }
    }


    private void Update()
    {
        
    }
}
