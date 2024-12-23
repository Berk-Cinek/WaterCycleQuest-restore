using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public void SaveGame(string sceneName)
    {
        SaveData saveData = new SaveData();
        saveData.sceneName = sceneName;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.Log("No save file found!");
            return null;
        }
    }
}
