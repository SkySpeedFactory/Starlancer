using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private string saveFilePath;
    private DirectoryInfo directoryPath;
    private FileInfo[] saveFileArray;


    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/Saves";
        directoryPath = new DirectoryInfo(saveFilePath);
       
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        SaveManager.Instance.TogglePauseMenu();
    }

    public void SaveButton()
    {
        Time.timeScale = 1;
        SaveManager.Instance.SaveGame();
        SaveManager.Instance.TogglePauseMenu();
        //SaveManager.Instance.isPaused = false;
        //Destroy(this.transform.parent.gameObject);
    }

    public void OpenOptionsButton()
    {
        
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        SaveManager.Instance.isPaused = false;
        //SaveManager.Instance.TogglePauseMenu();
        Destroy(this.gameObject);
    }

    public void ExitGameButton()
    {
        Time.timeScale = 1;
        SaveManager.Instance.isPaused = false;
        Application.Quit();
        //SaveManager.Instance.TogglePauseMenu();
        Destroy(this.gameObject);
    }
}
