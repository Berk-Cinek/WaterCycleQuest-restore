using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
   public GameObject MainMenuPanel;

    public void YesButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void NoButton()
    {
        MainMenuPanel.SetActive(false);
    }
}