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

    // Method to move a random item to the spawn point with a delay
    public void SpawnRandomItem()
    {
        if (remainingItems.Count == 0)
        {
            Debug.Log("All items have been spawned.");
            return;
        }

        // Prevent spawning multiple items at the same time
        if (!isSpawning)
        {
            StartCoroutine(SpawnItemWithDelay());
        }
    }

    // Coroutine to add a delay before spawning the item
    IEnumerator SpawnItemWithDelay()
    {
        isSpawning = true;

        // Wait for the specified delay
        yield return new WaitForSeconds(spawnDelay);

        // Select a random item from the remaining items
        int randomIndex = Random.Range(0, remainingItems.Count);
        GameObject randomItem = remainingItems[randomIndex];

        // Move the selected item to the spawn point
        randomItem.transform.position = spawnPoint.position;

        // Remove the item from the list so it isn't selected again
        remainingItems.RemoveAt(randomIndex);

        isSpawning = false;  // Allow spawning again after the delay
    }
}
