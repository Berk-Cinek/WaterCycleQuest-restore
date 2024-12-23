using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject SettingsCanvas;
    public GameObject pauseMenuUI;
    public GameObject QuitMenuPanel;
    public GameObject MainMenuPanel;
    public GameObject SaveSuccessPanel;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (ChatBoxController.IsChatBoxActive)
            {
                Debug.Log("Chat box is active. ESC will not toggle pause menu.");
                return; 
            }

            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Resuming game...");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    void Pause()
    {
        Debug.Log("Pausing game...");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        MainMenuPanel.SetActive(true);
    }

    public void Settings()
    {
        SettingsCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        QuitMenuPanel.SetActive(true);
    }

    public void Save()
    {
        SaveSuccessPanel.SetActive(true);
    }
}