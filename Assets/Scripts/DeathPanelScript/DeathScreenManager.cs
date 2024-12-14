using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private NewPlayerMovement player;
    [SerializeField] private Canvas deathCanvas;

    private void Start()
    {
        if (deathCanvas != null)
        {
            deathCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        Debug.Log("dsm Player Health: " + player.health);
        if (player != null && player.health <=0)
        {
            ShowDeathScreen();
        }
    }

    private void ShowDeathScreen()
    {
        if(deathCanvas != null)
        {
            deathCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
