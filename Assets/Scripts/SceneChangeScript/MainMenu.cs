using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject SettingsCanvas;

    public void StartGame()
    {
        SceneManager.LoadScene("Facility");
    }

    public void LoadGame()
    {
        gameManager.LoadGame();
    }

    public void Settings()
    {
        SettingsCanvas.SetActive(true);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
