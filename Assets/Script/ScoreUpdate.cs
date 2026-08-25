using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour
{
    private Text scoreText;
    public static ScoreUpdate Instance;

    private void Awake()
    {
        scoreText = GetComponent<Text>();
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        GameManager.Instance.onScoreChange += UpdateScore; //dang ki su kien
    }
    
    void OnDisable()
    {
        GameManager.Instance.onScoreChange -= UpdateScore; //dang ki su kien
    }

    public void UpdateScore(int score)
    {
        //cap nhat
        Debug.Log("cong diem!!");
        Debug.Log("GameManager.Instance.score cu = " + GameManager.Instance.score);
        GameManager.Instance.score += score;
        Debug.Log("GameManager.Instance.score moi = " + GameManager.Instance.score);
        Debug.Log("score = " + score);
        scoreText.text = "Score: " + GameManager.Instance.score;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
