using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    public List<GameObject> items;          // List of items already in the scene
    public List<Transform> itemPositions;   // Original positions of each item
    public Transform spawnPoint;            // The location where the items should appear
    public float spawnDelay = 2f;           // Delay in seconds before spawning the item

    private List<GameObject> remainingItems;  // To track which items are left to spawn
    private bool isSpawning = false;          // To prevent multiple spawns at the same time
    private Queue<int> spawnQueue = new Queue<int>(); // Queue for pending item spawns

    public delegate void ItemSpawnedCallback(); // Delegate for a callback after an item is spawned

    void Start()
    {
        // Ensure that each item has a corresponding position
        if (itemPositions.Count != items.Count)
        {
            Debug.LogError("The number of item positions must match the number of items.");
            return;
        }

        // Move each item to its original position initially (or hide them elsewhere if needed)
        for (int i = 0; i < items.Count; i++)
        {
            items[i].transform.position = itemPositions[i].position;
        }

        // Copy the list of items to remainingItems to keep track of the randomization
        remainingItems = new List<GameObject>(items);
    }

    // Method to queue a random item to be moved to the spawn point
    public void SpawnRandomItem(ItemSpawnedCallback callback)
    {
        if (remainingItems.Count == 0)
        {
            Debug.Log("All items have been spawned.");
            return;
        }

        // If an item is already spawning, queue the request
        if (isSpawning)
        {
            Debug.Log("Item is already spawning. Queueing the next spawn.");
            spawnQueue.Enqueue(Random.Range(0, remainingItems.Count));
        }
        else
        {
            StartCoroutine(SpawnItemWithDelay(Random.Range(0, remainingItems.Count), callback));
        }
    }

    // Coroutine to add a delay before spawning the item
    IEnumerator SpawnItemWithDelay(int randomIndex, ItemSpawnedCallback callback)
    {
        isSpawning = true;

        // Wait for the specified delay
        yield return new WaitForSeconds(spawnDelay);

        if (randomIndex < remainingItems.Count)
        {
            GameObject randomItem = remainingItems[randomIndex];

            // Move the selected item to the spawn point
            randomItem.transform.position = spawnPoint.position;

            // Remove the item from the list so it isn't selected again
            remainingItems.RemoveAt(randomIndex);

            Debug.Log("Item spawned.");
        }

        // Check if there are more items in the queue to be spawned
        if (spawnQueue.Count > 0)
        {
            int nextIndex = spawnQueue.Dequeue();
            StartCoroutine(SpawnItemWithDelay(nextIndex, callback));
        }
        else
        {
            // Reset the isSpawning flag if there are no more items in the queue
            isSpawning = false;

            // Call the callback to notify that spawning is done
            callback?.Invoke();
        }
    }
}
