using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public SaveSystem saveSystem;
    public Vector3 initialPlayerPosition = new Vector3(0, 0, 0);

    public void LoadGame()
    {

        SaveData saveData = saveSystem.LoadGame();

        if (saveData != null)
        {
            SceneManager.LoadScene(saveData.sceneName);
        }
        else
        {
            Debug.Log("No save file found, starting fresh.");
            SceneManager.LoadScene("Facility");
        }

        player.transform.position = initialPlayerPosition;
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();
        saveData.sceneName = SceneManager.GetActiveScene().name;

        saveSystem.SaveGame(saveData.sceneName);
    }
}
