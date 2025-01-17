﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void LevelReset()
    {
        StageStatic.stage.ResetPlay();
        Loader.LoadStage(StageStatic.stage.name, !StageStatic.stage.use_install_folder, false);
    }

    public void LoadMainMenue()
    {
        //not over SceneManager.LoadScene as MainMenue is too light to need to load over a LoadingScreen
        SceneManager.LoadScene("MainMenue");
    }

    public void LoadStartScreen()
    {
        StartServer.process.Kill(); // null reference exception if Server started manually
        SceneManager.LoadScene(0);
    }

    public void OnApplicationQuit()
    {
        StartServer.process.Kill(); // null reference exception if Server started manually
    }
}
