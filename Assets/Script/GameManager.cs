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
    public string currentScene;
    public string[] level = new[]
    {
        "Level01", 
        "Level02",
        "Level03"
    };
    public int score  {
        get;
        set;
    }

    public float hp = 17;
    public int maxHP = 17;
    //score: diem player dat duoc sau moi lan giet duoc yeu quai
    public bool isGameOver = false;
    public Text scoreCamera;
    private bool scoreAtStart = false;
    
    //khai bao action, bao khi diem thay doi
    public Action<int> onScoreChange;

    private PlayerController playerController;

    
    void Awake()
    {
        //neu co GameManager roi
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        //neu chua co
        Instance = this;
        DontDestroyOnLoad(gameObject);
        score = 0;
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

    private void Update()
    {
        Debug.Log("numberOfScene: " + numberOfScene);
    }

    public void ReachCheckPoint(int checkPoint)
    {
        SoundManager.Instance.currentSound = SoundManager.SoundID.ReachCheckpoint;
        //luu lai du lieu
        SaveCheckPoint(level[checkPoint], checkPoint, hp, this.score);
        //chuan bi chuyen sang scene tiep theo
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
    
    public void SaveCheckPoint(string sceneName, int checkPoint, float hp, int score)
    {
       SaveData saveData = new SaveData();
       saveData.sceneName = sceneName;
       saveData.checkPoint = checkPoint;
       saveData.health = hp;
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
            this.score = 0;
        onScoreChange?.Invoke(this.score);
        if (this.score <= 0)
            gameOver();
    }

    public void gameOver()
    {
        isGameOver = true;
        //instantiate gameover prefab hoac dung game
    }

    public void Next()
    {
        //cap nhat numberOfScene
        //Debug.Log("level.Length: " +  level.Length);
        
        //Debug.Log("numberOfScene tham so: " +  numberOfScene);
        if (numberOfScene < level.Length - 1)
        {
            numberOfScene = numberOfScene + 1;
            Debug.Log("cong 1 vao scene, scene = " + numberOfScene );
            string tmp = level[numberOfScene];
            SceneManager.LoadScene(tmp);
        }
        else
        {
            //Da hoan thanh game
        }
    }
}
