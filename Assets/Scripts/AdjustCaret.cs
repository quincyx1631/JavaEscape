using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdjustCaret : MonoBehaviour
{
    void Update()
    {
        // Check if there's a currently selected TMP_InputField
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            TMP_InputField selectedInputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();

            // If it's a TMP_InputField, adjust the caret
            if (selectedInputField != null)
            {
                // Adjust the caret width to a small value (like 0.002, though typically it expects an int)
                selectedInputField.caretWidth = Mathf.RoundToInt(0.002f); // Using Mathf.RoundToInt to convert the float to an int

                // Optionally, you can also tweak the caret color or other properties if needed
            }
        }
    }
}
