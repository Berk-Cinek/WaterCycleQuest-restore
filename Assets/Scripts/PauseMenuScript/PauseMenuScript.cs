using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject SettingsCanvas;
    public GameObject pauseMenuUI;
    public GameObject QuitMenuPanel;
    public GameObject MainMenuPanel;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    void Pause()
    {
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
}
