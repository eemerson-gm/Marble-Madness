﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    
    public void LoadScene(int level)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(level);
    }
    public void StopGame()
    {
        Application.Quit();
    }

}
