using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
public class Hurt : MonoBehaviour
{
    private SpriteRenderer sr;

    private void OnEnable()
    {
        GameManager.Instance.onHpChange += getHurt;
    }

    private void OnDisable()
    {
        GameManager.Instance.onHpChange -= getHurt;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    
    IEnumerator Hurt_interface()
    {
        sr.enabled = false; //mat
        yield return new WaitForSeconds(0.1f);
        sr.enabled = true; //hien
        yield return new WaitForSeconds(0.1f);
        sr.enabled = false; //mat
        yield return new WaitForSeconds(0.1f);
        sr.enabled = true; //hien
    }
    
    public void getHurt(int delete)
    {
        StartCoroutine(Hurt_interface());
    }
}
