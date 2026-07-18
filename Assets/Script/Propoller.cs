using System;
using UnityEngine;


public class Propoller : MonoBehaviour
{
    public Action<float> HPEnemyChange;
    float blood = 1f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("bi ban");
            takeDamage();
        }
    }
    
    public void takeDamage()
    {
        Debug.Log("tru 0.2f mau cua propoller");
        blood = blood - 0.2f;
        HPEnemyChange?.Invoke(-0.2f);
        //HPbarController
        if (blood <= 0)
        {
            Destroy(gameObject);
        }
    }
}
