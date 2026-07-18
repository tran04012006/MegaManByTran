using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour
{
    private Text scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText = GetComponent<Text>();
        GameManager.Instance.onScoreChange += UpdateScore;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
