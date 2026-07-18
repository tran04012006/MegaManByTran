using System;
using UnityEngine;

public class ToxicNoBarrel : MonoBehaviour
{
    private Transform firePoint;

    public bool isRight = false;

    public GameObject bullet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Fire();
    }
    
    

    public void Fire()
    {
        //chon vi tri xuat hien vien dan
        if (isRight == false) //ben trai
            firePoint = gameObject.transform.GetChild(0);
        else
        {
            firePoint = gameObject.transform.GetChild(1);
        }
        //tao vien dan
        GameObject newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        //di lay cai script BulletController cua thang gameobject nay
        BulletController bc = newBullet.gameObject.GetComponent<BulletController>();
        //di vao ham setDirection trong script bulletController
        bc.setDirection(isRight);
        Destroy(newBullet, 2f);
    }
}
