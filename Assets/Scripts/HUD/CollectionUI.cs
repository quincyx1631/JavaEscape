using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CollectionItem
{
    public List<string> itemNames; // List of item names
    public int total; // Total number of this item to collect
    public int collected; // Number of this item collected so far
    public string collectedText; // Text to display when the item is collected
    public TextMeshProUGUI itemText; // Reference to the TextMeshProUGUI component for this item
}

public class CollectionUI : MonoBehaviour
{
    public List<CollectionItem> collectionItems; // List of items to collect

    private Dictionary<string, CollectionItem> itemDictionary;

    void Start()
    {
        // Initialize itemDictionary for quick lookups
        itemDictionary = new Dictionary<string, CollectionItem>();

        foreach (var item in collectionItems)
        {
            if (item.itemText != null)
            {
                // Add items to the dictionary
                foreach (var name in item.itemNames)
                {
                    itemDictionary[name] = item;
                }
                // Initialize UI texts
                UpdateItemText(item);
            }
            else
            {
                Debug.LogError($"ItemText for {item.itemNames[0]} is not assigned!");
            }
        }
    }

    // Call this method when an item is collected
    public void OnItemCollected(string itemName)
    {
        if (itemDictionary.TryGetValue(itemName, out var item))
        {
            if (item.collected < item.total) // Ensure not to exceed total number
            {
                item.collected++;
                UpdateItemText(item);
                Debug.Log($"Item collected: {itemName} - {item.collected}/{item.total}");
            }
            else
            {
                Debug.LogWarning($"{itemName} has already reached its total count.");
            }
        }
        else
        {
            Debug.LogWarning($"Item {itemName} not found in the collection.");
        }
    }

    // Update the UI text for a specific CollectionItem
    private void UpdateItemText(CollectionItem item)
    {
        if (item.itemText != null)
        {
            // Base text display
            string displayText = $"{item.collectedText}: {item.collected}/{item.total}";

            // Check if all items are collected
            if (item.collected >= item.total)
            {
                // Apply strikethrough and alpha color (50% transparency)
                displayText = $"<s><color=#FFFFFF80>{displayText}</color></s>";
            }

            item.itemText.text = displayText;
        }
    }

    public bool AreAllItemsCollected()
    {
        foreach (var item in collectionItems)
        {
            if (item.collected < item.total)
            {
                return false; // Not all items are collected
            }
        }
        return true; // All items are collected
    }
}
