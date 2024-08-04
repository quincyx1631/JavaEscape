using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] buttons;
    private SaveData saveData;
    private int slot = 0; // Use a default slot

    private void Awake()
    {
        saveData = SaveLoadManager.instance.LoadGame(slot) ?? new SaveData();
        UpdateLevelButtons();
    }

    private void UpdateLevelButtons()
    {
        // Disable all buttons initially
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        // Enable buttons for completed levels and the next level to complete
        for (int i = 0; i <= saveData.completedLevels.Count && i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }
}
