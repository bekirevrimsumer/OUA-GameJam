using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainManu;
    public GameObject settingsMenu;
    public GameObject infoPage;
    
    public void PlayButton()
    {
        SceneManager.LoadScene("GameScene");
        
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
    
    
}
