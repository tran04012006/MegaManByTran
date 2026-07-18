using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool isShowStory = false;
    private AudioSource _audioSource;

    public GameObject story;
    public GameObject closeStory;

    private void Start()
    {
        closeStory.SetActive(false); //ban dau thi tat nut nay
        story.SetActive(false); //ban dau thi xoa story
        _audioSource = GetComponent<AudioSource>();
    }

    // khai bao story xong thi ko can getcomponent, vi ta chi can thao tac tren GameObject
    // ta chi getcomponent khi can lay mot component cua gameobject
    // vi du, neu A co a1,a2,a3, ta can a1 thi getcomponent, neu chi can A thi ko can getcomponent
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        Debug.Log("Play");
        //truyen vao ten cua scene
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {        
        _audioSource.Play();
        Debug.Log("Exit");

    }

    public void Setting()
    {
        Debug.Log("Setting");
        _audioSource.Play();
        //chuyen sang scene Setting
    }
    
    public void Story()
    {
        _audioSource.Play();
        Debug.Log("Story");
        //tim trong tat ca GameObject dang active trong scene, cai nao co tag la story
        //thi gan cai do vao story
        if (story.activeSelf == false)
        {
            //neu story dang active (co dau tick) thi dat no lai thanh false
            story.SetActive(true);
            closeStory.SetActive(true);
        }
        
    }

    public void CloseStory()
    {
        _audioSource.Play();
        if (story.activeSelf == true)
        {
            //neu story dang active (co dau tick) thi dat no lai thanh false
            story.SetActive(false);
            closeStory.SetActive(false);

        }
    }
    
}
