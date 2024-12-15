using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private NewPlayerMovement player;
    [SerializeField] private GameObject deathPanel;

    private void Start()
    {
        if (deathPanel != null)
        {
            deathPanel.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    private void Update()
    {
        Debug.Log("Update çalýþýyor, player health: " + player.health);

        if (player.health <= 0)
        {
            Debug.Log("Player öldü.");
            ShowDeathScreen();
        }
    }

    private void ShowDeathScreen()
    {
        if(deathPanel != null)
        {
            deathPanel.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RetryLevel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void MainMenu()
    {

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}