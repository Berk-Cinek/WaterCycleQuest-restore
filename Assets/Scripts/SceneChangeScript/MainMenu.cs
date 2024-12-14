using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{ 
    public GameObject SettingsCanvas;

    public void StartGame()
    {
        SceneManager.LoadScene("Facility");
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings()
    {
        SettingsCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
