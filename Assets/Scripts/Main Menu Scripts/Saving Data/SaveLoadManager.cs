using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(SaveData data, int slot)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile" + slot + ".json", json);
    }

    public SaveData LoadGame(int slot)
    {
        string path = Application.persistentDataPath + "/savefile" + slot + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null;
    }

    public void DeleteSave(int slot)
    {
        string path = Application.persistentDataPath + "/savefile" + slot + ".json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
