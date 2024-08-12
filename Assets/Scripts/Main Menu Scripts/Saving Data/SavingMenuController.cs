using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingMenuController : MonoBehaviour
{
    public InputField nameInputField;
    public Text[] slotNames; // Array of Text components to display slot names
    private int selectedSlot = -1;

    private void Start()
    {
        LoadSlotNames();
    }

    public void SelectSlot(int slot)
    {
        selectedSlot = slot;
        Debug.Log("Selected slot: " + slot);
    }

    public void SaveGame()
    {
        if (selectedSlot == -1)
        {
            Debug.LogError("No slot selected for saving.");
            return;
        }

        string playerName = nameInputField.text;
        SaveData data = new SaveData
        {
            playerName = playerName,
            completedLevels = new List<int>() // Initially empty
        };

        SaveLoadManager.instance.SaveGame(data, selectedSlot);
        UpdateSlotName(selectedSlot, playerName);
    }

    public void LoadGame()
    {
        if (selectedSlot == -1)
        {
            Debug.LogError("No slot selected for loading.");
            return;
        }

        SaveData data = SaveLoadManager.instance.LoadGame(selectedSlot);

        if (data != null)
        {
            // Handle loading the data into the game
            Debug.Log("Loaded data: Player Name " + data.playerName + ", Completed Levels " + string.Join(", ", data.completedLevels));
        }
        else
        {
            Debug.Log("No save data found for slot " + selectedSlot);
        }
    }

    public void DeleteSave()
    {
        if (selectedSlot == -1)
        {
            Debug.LogError("No slot selected for deleting.");
            return;
        }

        SaveLoadManager.instance.DeleteSave(selectedSlot);
        UpdateSlotName(selectedSlot, string.Empty);
    }

    private void LoadSlotNames()
    {
        for (int i = 0; i < slotNames.Length; i++)
        {
            SaveData data = SaveLoadManager.instance.LoadGame(i);
            if (data != null)
            {
                slotNames[i].text = data.playerName;
            }
            else
            {
                slotNames[i].text = "Empty";
            }
        }
    }

    private void UpdateSlotName(int slot, string name)
    {
        if (slot >= 0 && slot < slotNames.Length)
        {
            slotNames[slot].text = string.IsNullOrEmpty(name) ? "Empty" : name;
        }
    }
}
