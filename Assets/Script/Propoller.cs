using System;
using UnityEngine;


public class Propoller : MonoBehaviour
{
    private EnemyController enemeyController;
    float blood = 1f;
    public EnemyItem _enemyItem;
    private float damage; //10
    private float hp; //10
    private const float damageOfPlayer = 5;
    public AudioClip propollerSound;
    private AudioSource _audioSource;

    private void Start()
    {
        enemeyController = GetComponent<EnemyController>();
        damage = _enemyItem.damage;
        hp = _enemyItem.hp;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = propollerSound;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("tru mau cua propoller");
            Debug.Log("bi ban");
        }
    }

   
}
