using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;
    public CollectionUI collectionUI; // Reference to the CollectionUI

    private List<Interactables> collectedObjects = new List<Interactables>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MarkAsCollected(Interactables collectible)
    {
        if (!collectedObjects.Contains(collectible))
        {
            collectedObjects.Add(collectible);
            Debug.Log($"{collectible.name} has been collected!");

            // Ensure collectionUI is assigned before calling its method
            if (collectionUI != null)
            {
                collectionUI.OnItemCollected(collectible.name);
            }
            else
            {
                Debug.LogError("collectionUI is not assigned!");
            }
        }
    }

    public bool IsCollected(Interactables collectible)
    {
        return collectedObjects.Contains(collectible);
    }
}
