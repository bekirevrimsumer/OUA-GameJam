using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOnMenuManager : MonoBehaviour
{

    public GameObject menuInGame;
    
    public GameObject music;
    public bool movedVar = MainMenuManager.isMuted;
    
    public void Awake()
    {
        music.SetActive(!MainMenuManager.isMuted);
    }

    /*public void Switch(bool var)
    {
        movedVar = !movedVar;
    }*/
    
    /*public void IsItMoved()
    {
        if (movedVar != MainMenuManager.isMuted)
            movedVar = MainMenuManager.isMuted;
        //Switch(movedVar);
    }*/

    public void OnGameMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuInGame.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        menuInGame.SetActive(false);
        Time.timeScale = 1;
    }

    public void Mute()  //zaten sessizse ekle
    {
        //IsItMoved();
        music.SetActive(false);
        movedVar = true;
    }

    public void Unmute() //zaten sesliyse ekle
    {
        //sItMoved();
        music.SetActive(true);
        movedVar = false;
    }
    
    void Update()
    {
        OnGameMenu();
        //IsItMoved();
    }
}
