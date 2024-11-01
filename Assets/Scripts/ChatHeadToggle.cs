using UnityEngine;

public class ChatHeadToggle : MonoBehaviour
{
    public GameObject conversationUI; // Reference to the conversation UI GameObject
    public GameObject notificationUI; // Reference to the notification UI GameObject
    public GameObject additionalUI;   // Reference to the additional UI to toggle

    private bool isConversationVisible = false; // Track the visibility state

    public void ToggleConversation()
    {
        isConversationVisible = !isConversationVisible;
        Debug.Log($"{gameObject.name} toggling conversationUI: {conversationUI.name} to {isConversationVisible}");

        // Toggle conversation UI visibility
        if (conversationUI != null)
        {
            conversationUI.SetActive(isConversationVisible);
            Debug.Log($"{gameObject.name} Conversation UI is now: {(isConversationVisible ? "Visible" : "Hidden")}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Conversation UI reference is missing!");
        }

        // Toggle additional UI visibility
        if (additionalUI != null)
        {
            additionalUI.SetActive(isConversationVisible);
            Debug.Log($"{gameObject.name} Additional UI is now: {(isConversationVisible ? "Visible" : "Hidden")}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Additional UI reference is missing!");
        }

        // Hide the notification when the conversation is opened
        if (isConversationVisible)
        {
            if (notificationUI != null)
            {
                notificationUI.SetActive(false);
                Debug.Log($"{gameObject.name}: Notification UI hidden.");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: Notification UI reference is missing!");
            }
        }
    }
}
