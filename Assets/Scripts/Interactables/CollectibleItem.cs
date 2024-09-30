using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public bool IsCollected { get; private set; } = false;  // Flag to track if the item is collected

    // Call this method when the item is collected by the player
    public void CollectItem()
    {
        IsCollected = true;
        gameObject.SetActive(false);  // Disable the item after it's collected
    }
}
