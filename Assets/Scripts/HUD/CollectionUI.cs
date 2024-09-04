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

        // Ensure all books are visible at the start
        ShowAllBooks();
    }

    public void OnItemCollected(string itemName)
    {
        if (itemDictionary.TryGetValue(itemName, out var item))
        {
            if (item.collected < item.total)
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

    public void InitializeQuizzes(List<string> quizNames)
    {
        // Deactivate all quizzes initially
        foreach (var item in itemDictionary.Values)
        {
            if (item.itemNames.Contains("Quiz")) // Assuming quizzes are labeled with "Quiz"
            {
                item.itemText.gameObject.SetActive(false); // Hide the quiz item
                item.collected = 0;
                UpdateItemText(item);
            }
        }

        // Activate and reset only the quiz items in the quizNames list
        foreach (var quizName in quizNames)
        {
            if (itemDictionary.TryGetValue(quizName, out var item))
            {
                if (item.itemText != null)
                {
                    item.itemText.gameObject.SetActive(true); // Show the quiz item
                    item.collected = 0; // Reset the collected count
                    UpdateItemText(item);
                }
                else
                {
                    Debug.LogError($"ItemText for {quizName} is null!");
                }
            }
            else
            {
                Debug.LogWarning($"Quiz {quizName} not found in the collection.");
            }
        }
    }

    private void ShowAllBooks()
    {
        foreach (var item in collectionItems)
        {
            if (!item.itemNames.Contains("Quiz")) // Assuming books are not labeled with "Quiz"
            {
                if (item.itemText != null)
                {
                    item.itemText.gameObject.SetActive(true); // Show all book items
                    UpdateItemText(item);
                }
                else
                {
                    Debug.LogError($"ItemText for book {item.itemNames[0]} is null!");
                }
            }
        }
    }

    private void UpdateItemText(CollectionItem item)
    {
        if (item.itemText != null)
        {
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
