using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPbarController : MonoBehaviour
{
    private Slider slider;

    public EnemyController enemyController;

    public GameObject fill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
        EnemyController enemyController = GetComponent<EnemyController>();
        slider.maxValue = enemyController.hp;
    }

    private void OnEnable()
    {
        enemyController.HPEnemyChange += UpdateHP;
    }

    private void OnDisable()
    {
        enemyController.HPEnemyChange -= UpdateHP;    
    }

    IEnumerator IsGoingToDie()
    {
        fill.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        fill.SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }
    
    public void UpdateHP(float currentHP)
    {
        //cap nhat giao dien cho hp hien tai
        slider.value = currentHP;
        //Debug.Log("slider value: " + slider.value);
        if (slider.value > slider.maxValue)
        {
            slider.value = slider.maxValue;
        }
        else if (slider.value < slider.minValue)
        {
            slider.value = slider.minValue;
        }
        
        //khi sap can mau
        if (slider.value <= 0.3)
        {
            //cho hien tuong chop tat
            StartCoroutine(IsGoingToDie());
        }
    }

    public void fullHP()
    {
        slider.value = GameManager.Instance.maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
