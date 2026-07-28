using System;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }
    public int numberOfScene = 0;
    public string[] level = new[]
    {
        "level01", 
        "level02",
        "level03"
    };
    public int score = 0;

    public int heath = 17;

    public int maxHP = 17;
    //score: diem player dat duoc sau moi lan giet duoc yeu quai
    public bool isGameOver = false;
    public Text scoreCamera;
    private bool scoreAtStart = false;
    
    //khai bao action, bao khi diem thay doi
    public Action<int> onScoreChange;

    //khai bao action khi hp thay doi 
    public Action<int> onHpChange;
    
    void Awake()
    {
        Instance = this;
        /*
        //DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            
        }
        else
        {
            Destroy(gameObject);
        }
        //cap nhat score ban dau la 0
        */
    }
    
    private void OnEnable()
    {
        Debug.Log("dang ki su kien thay doi score");
        SceneManager.sceneLoaded += onSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoad;
    }

    public void ReachCheckPoint(int checkPoint)
    {
        //luu lai du lieu
        SaveCheckPoint(level[checkPoint], checkPoint, heath, this.score);
        //chuan bi chuyen sang scene tiep theo
        if (checkPoint <= level.Length - 1)
        {
            //chuyen sang scene tiep theo 
            checkPoint++;
            SceneManager.LoadScene(level[checkPoint]);
        }
        else
        {
            //neu het man choi
            Debug.Log("Da hoan thanh game");
        }
        //cap nhat data trong scene moi
    }

    void onSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("onSceneLoad");
        //unity tu goi class nay
        
        //lay ra noi chua file json
        string path =  Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path) == true)
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            
            //cap nhat score
            ScoreUpdate scoreUI = FindFirstObjectByType<ScoreUpdate>();
            if (scoreUI != null)
            {
                Debug.Log("score trong scene 2 = " + saveData.score);
                scoreUI.UpdateScore(saveData.score);
                //cap nhat lai score trong GameManger
                this.score = saveData.score;
            }            
            //delete de no ko con giu lai data cu nua
            File.Delete(path);
        }
        else
        {
            //cap nhat score
            
        }
    }
    
    public void SaveCheckPoint(string sceneName, int checkPoint, int health, int score)
    {
       SaveData saveData = new SaveData();
       saveData.sceneName = sceneName;
       saveData.checkPoint = checkPoint;
       saveData.health = health;
       saveData.score = score;
       string json = JsonUtility.ToJson(saveData);
       string path = Application.persistentDataPath + "/SaveData.json";
       File.WriteAllText(path, json);
       Debug.Log("Saved checkpoint !!!!");
    }
    

    public void addScore(int score)
    {
        //tang score
        Debug.Log("cong 10d");
        this.score += score;
        //thong bao diem thay doi
        onScoreChange?.Invoke(this.score);
    }

    public void minusScore(int score)
    {
        this.score -= score;
        if (this.score <= 0)
            gameOver();
    }

    public void gameOver()
    {
        isGameOver = true;
        //instantiate gameover prefab hoac dung game
    }
    
}
