﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelController : MonoBehaviour
{  public void LoadLevel1()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }


}
