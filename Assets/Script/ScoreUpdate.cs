using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour
{
    private Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.onScoreChange += UpdateScore; //dang ki su kien
        //cap nhat ngay score khi vua moi bat dau
        UpdateScore(GameManager.Instance.score);
    }

    public void UpdateScore(int score)
    {
        //cap nhat
        scoreText.text = "Score: " + score;
        GameManager.Instance.score = score;
        Debug.Log("score = " + GameManager.Instance.score);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
