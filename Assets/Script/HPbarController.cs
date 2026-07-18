using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPbarController : MonoBehaviour
{
    private Slider slider;

    public Propoller propoller;

    public GameObject fill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
        propoller.HPEnemyChange += UpdateHP;
    }

    IEnumerator IsGoingToDie()
    {
        fill.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        fill.SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }
    
    void UpdateHP(float hp)
    {
        //cap nhat giao dien cho hp hien tai
        if (slider.value > 17)
        {
            slider.value = 17;
        }
        else if (slider.value < 0)
        {
            slider.value = 0;
        }
        else
        {
            slider.value += hp;
        }
        while (slider.value <= 0.3)
        {
            //cho hien tuong chop tat
            StartCoroutine(IsGoingToDie());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
