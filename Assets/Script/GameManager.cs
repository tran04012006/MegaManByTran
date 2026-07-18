using System;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    public int score = 0;

    public int heath;
    //score: diem player dat duoc sau moi lan giet duoc yeu quai
    public bool isGameOver = false;
    public Text scoreCamera;
    //khai bao action, bao khi diem thay doi
    public Action<int> onScoreChange;

    //khai bao action khi hp thay doi 
    public Action<int> onHpChange;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void addScore(int score)
    {
        //tang score
        Debug.Log("cong 10d");
        this.score += score;
        //thong bao diem thay doi
        onScoreChange?.Invoke(this.score);
    }

    public void gameOver()
    {
        isGameOver = true;
        //instantiate gameover prefab hoac dung game
    }
    
}
