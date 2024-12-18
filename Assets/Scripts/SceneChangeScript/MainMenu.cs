using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameManager gameManager;

    // Start a new game (loads the first level, e.g., Facility scene)
    public void StartGame()
    {
        SceneManager.LoadScene("Facility");
    }

    // Load the saved game (reloads the last saved scene and resets the player to the initial position)
    public void LoadGame()
    {
        gameManager.LoadGame();
    }

    // Return to the main menu
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
