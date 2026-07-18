using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingControler : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider volumeSliderSPX;
    
    public Toggle muteToggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        volumeSliderSPX.value = PlayerPrefs.GetFloat("fbx");
        muteToggle.isOn = PlayerPrefs.GetInt("mute") == 1 ? true : false;
    }

    public void Save()
    {
        float volume = volumeSlider.value;
        float fbx = volumeSliderSPX.value;
        bool mute = muteToggle.isOn ? true : false;

        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("fbx", fbx);
        PlayerPrefs.SetInt("mute", mute ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
