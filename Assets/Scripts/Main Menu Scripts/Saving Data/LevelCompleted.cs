using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleted : MonoBehaviour
{
    public int levelId; // Set this in the inspector for each level

    private void Start()
    {
        SaveData saveData = SaveLoadManager.instance.LoadGame(0) ?? new SaveData();
        if (!saveData.completedLevels.Contains(levelId))
        {
            saveData.completedLevels.Add(levelId);
            SaveLoadManager.instance.SaveGame(saveData, 0);
        }
    }
}
