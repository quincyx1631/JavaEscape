using UnityEngine;

public class LetterBox : MonoBehaviour
{
    private Vector3 originalScale; // To store the original scale of the item
    public char letter; // The letter contained in this LetterBox

    // Called when the item is first picked up
    void Awake()
    {
        // Store the original scale at the start
        originalScale = transform.localScale;
    }

    // Function to get the original scale
    public Vector3 GetOriginalScale()
    {
        return originalScale;
    }
}
