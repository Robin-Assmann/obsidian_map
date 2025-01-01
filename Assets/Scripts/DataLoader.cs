using System.IO;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    // Create a field for the save file.
    static string SaveDataPathFile => Application.persistentDataPath + "/savedata.json";



    public SaveData saveData;

    public void Init()
    {
        // Does the file exist?
        if (File.Exists(SaveDataPathFile))
        {
            string fileContents = File.ReadAllText(SaveDataPathFile);
            saveData = JsonUtility.FromJson<SaveData>(fileContents);
            return;
        }

        saveData = new SaveData();
    }

    public void Save()
    {
        File.WriteAllText(SaveDataPathFile, JsonUtility.ToJson(saveData));
    }
}