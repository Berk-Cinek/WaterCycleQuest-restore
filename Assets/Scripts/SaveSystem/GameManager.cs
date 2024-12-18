using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public SaveSystem saveSystem;  // Reference to the SaveSystem
    public Vector3 initialPlayerPosition = new Vector3(0, 0, 0); // The initial spawn position of the player

    // Call this to load the saved game
    public void LoadGame()
    {
        // Load the saved data
        SaveData saveData = saveSystem.LoadGame();

        if (saveData != null)
        {
            // Reload the saved scene (the player was last in)
            SceneManager.LoadScene(saveData.sceneName);
        }
        else
        {
            Debug.Log("No save file found, starting fresh.");
            // If no save exists, start from the beginning (e.g., Facility scene)
            SceneManager.LoadScene("Facility");
        }

        // After the scene is loaded, set the player position to the initial spawn point
        // This ensures the player always starts at the same position
        player.transform.position = initialPlayerPosition;
    }

    // Call this to save the game
    public void SaveGame()
    {
        // Save the current scene (without saving player position, since we're resetting it)
        SaveData saveData = new SaveData();
        saveData.sceneName = SceneManager.GetActiveScene().name;

        saveSystem.SaveGame(saveData.sceneName);
    }
}
