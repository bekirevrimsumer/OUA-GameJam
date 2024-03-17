using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainManu;
    public GameObject settingsMenu;
    public GameObject infoPage;
    public GameObject howToPlay;

    public GameObject music;
    public static bool isMuted = false;
    
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsButton()
    {
        mainManu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    
    public void InfoButton()
    {
        mainManu.SetActive(false);
        infoPage.SetActive(true);
    }
    
    public void HowToPlay()
    {
        mainManu.SetActive(false);
        howToPlay.SetActive(true);
    }
    
    public void Mute()
    {
        music.SetActive(false);
        isMuted = true;
    }

    public void Unmute()
    {
        music.SetActive(true);
        isMuted = false;
    }

    public void cancelSettings()
    {
        settingsMenu.SetActive(false);
        mainManu.SetActive(true);
    }
    
    public void cancelInfo()
    {
        infoPage.SetActive(false);
        mainManu.SetActive(true);
    }

    public void cancelHowToPlay()
    {
        howToPlay.SetActive(false);
        mainManu.SetActive(true);
    }
}
