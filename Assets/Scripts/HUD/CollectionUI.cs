using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CollectionItem
{
    public List<string> itemNames = new List<string>(); // Initialize the list
    public int total;
    public int collected;
    public string collectedText;
    public TextMeshProUGUI itemText;

    // Validate the collection item
    public bool IsValid()
    {
        if (itemNames == null || itemNames.Count == 0)
        {
            Debug.LogError("Collection item has no names assigned!");
            return false;
        }

        if (itemText == null)
        {
            Debug.LogError($"TextMeshProUGUI component not assigned for item: {itemNames[0]}");
            return false;
        }

        if (string.IsNullOrEmpty(collectedText))
        {
            Debug.LogError($"CollectedText is empty for item: {itemNames[0]}");
            return false;
        }

        if (total <= 0)
        {
            Debug.LogError($"Total must be greater than 0 for item: {itemNames[0]}");
            return false;
        }

        return true;
    }
}

public class CollectionUI : MonoBehaviour
{
    public List<CollectionItem> collectionItems = new List<CollectionItem>();
    private Dictionary<string, CollectionItem> itemDictionary;
    private bool isInitialized = false;

    void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        try
        {
            itemDictionary = new Dictionary<string, CollectionItem>();

            // Validate collection items
            if (collectionItems == null || collectionItems.Count == 0)
            {
                Debug.LogError("CollectionItems list is null or empty!");
                return;
            }

            // Validate and add items to dictionary
            foreach (var item in collectionItems)
            {
                if (!item.IsValid()) continue;

                foreach (var name in item.itemNames)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        Debug.LogError("Empty item name found!");
                        continue;
                    }

                    if (itemDictionary.ContainsKey(name))
                    {
                        Debug.LogError($"Duplicate item name found: {name}");
                        continue;
                    }

                    itemDictionary[name] = item;
                    Debug.Log($"Added item to dictionary: {name}");
                }

                // Reset collected count
                item.collected = 0;
                UpdateItemText(item);
            }

            isInitialized = true;
            Debug.Log("CollectionUI initialized successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error during initialization: {e.Message}\n{e.StackTrace}");
            isInitialized = false;
        }
    }

    void Start()
    {
        if (isInitialized)
        {
            ShowAllBooks();
        }
        else
        {
            Debug.LogError("CollectionUI failed to initialize properly!");
        }
    }

    public void InitializeQuizzes(List<string> quizNames)
    {
        if (!isInitialized)
        {
            Debug.LogError("CollectionUI not properly initialized! Attempting to reinitialize...");
            InitializeDictionary();
            if (!isInitialized) return;
        }

        if (quizNames == null)
        {
            Debug.LogError("QuizNames list is null!");
            return;
        }

        Debug.Log($"Initializing {quizNames.Count} quizzes");

        try
        {
            // First deactivate all existing quizzes
            foreach (var item in collectionItems)
            {
                if (item != null && item.itemNames != null && item.itemNames.Contains("Quiz"))
                {
                    if (item.itemText != null)
                    {
                        item.itemText.gameObject.SetActive(false);
                        item.collected = 0;
                        UpdateItemText(item);
                    }
                }
            }

            // Then activate specified quizzes
            foreach (var quizName in quizNames)
            {
                Debug.Log($"Processing quiz: {quizName}");

                if (string.IsNullOrEmpty(quizName))
                {
                    Debug.LogError("Empty quiz name found!");
                    continue;
                }

                if (!itemDictionary.TryGetValue(quizName, out var item))
                {
                    Debug.LogError($"Quiz {quizName} not found in itemDictionary!");
                    continue;
                }

                if (item.itemText == null)
                {
                    Debug.LogError($"ItemText component is null for quiz {quizName}!");
                    continue;
                }

                item.itemText.gameObject.SetActive(true);
                item.collected = 0;
                UpdateItemText(item);
                Debug.Log($"Successfully activated quiz: {quizName}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error during quiz initialization: {e.Message}\n{e.StackTrace}");
        }
    }

    public void OnItemCollected(string itemName)
    {
        if (!isInitialized)
        {
            Debug.LogError("CollectionUI not properly initialized!");
            return;
        }

        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Attempted to collect item with null or empty name!");
            return;
        }

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
                Debug.LogWarning($"{itemName} has already reached its total count ({item.total}).");
            }
        }
        else
        {
            Debug.LogError($"Item {itemName} not found in the collection dictionary!");
        }
    }

    private void ShowAllBooks()
    {
        try
        {
            foreach (var item in collectionItems)
            {
                if (item != null && item.itemNames != null && !item.itemNames.Contains("Quiz"))
                {
                    if (item.itemText != null)
                    {
                        item.itemText.gameObject.SetActive(true);
                        UpdateItemText(item);
                        Debug.Log($"Showed book: {item.itemNames[0]}");
                    }
                    else
                    {
                        Debug.LogError($"ItemText for book {item.itemNames[0]} is null!");
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error showing books: {e.Message}\n{e.StackTrace}");
        }
    }

    private void UpdateItemText(CollectionItem item)
    {
        if (item == null || item.itemText == null) return;

        try
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
        catch (System.Exception e)
        {
            Debug.LogError($"Error updating item text: {e.Message}\n{e.StackTrace}");
        }
    }

    public bool AreAllItemsCollected()
    {
        if (!isInitialized) return false;

        try
        {
            foreach (var item in collectionItems)
            {
                if (item != null && item.collected < item.total)
                {
                    return false;
                }
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error checking collected items: {e.Message}\n{e.StackTrace}");
            return false;
        }
    }

    // Helper method to validate setup
    public void ValidateSetup()
    {
        Debug.Log("Starting CollectionUI validation...");

        if (collectionItems == null || collectionItems.Count == 0)
        {
            Debug.LogError("No collection items defined!");
            return;
        }

        foreach (var item in collectionItems)
        {
            if (item == null)
            {
                Debug.LogError("Null collection item found!");
                continue;
            }

            Debug.Log($"Validating item: {(item.itemNames.Count > 0 ? item.itemNames[0] : "unnamed")}");

            if (item.itemNames == null || item.itemNames.Count == 0)
            {
                Debug.LogError("Item has no names assigned!");
                continue;
            }

            if (item.itemText == null)
            {
                Debug.LogError($"TextMeshProUGUI component missing for {item.itemNames[0]}");
                continue;
            }

            Debug.Log($"Item {item.itemNames[0]} validation successful");
        }

        Debug.Log("CollectionUI validation complete");
    }
}