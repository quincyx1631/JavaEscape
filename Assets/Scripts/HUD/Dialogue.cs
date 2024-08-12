using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class DialogueEntry
{
    public string characterName;
    public Sprite characterImage;
    [TextArea]
    public string dialogueText;
    public string voiceOverName; // Name of the voice-over in the AudioManager
}

public class Dialogue : MonoBehaviour
{
    public FirstPersonController controller;
    public Image characterImageUI;
    public TMP_Text characterNameUI;
    public TMP_Text dialogueTextUI;
    public Button nextButton;
    public Timer timer; // Reference to the Timer script or component

    public DialogueEntry[] dialogueEntries;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;

    private void Start()
    {
        DisplayDialogue();
        DisablePlayerControls();
        ShowCursor();
    }

    public void DisplayDialogue()
    {
        if (currentDialogueIndex < dialogueEntries.Length)
        {
            DialogueEntry entry = dialogueEntries[currentDialogueIndex];
            characterNameUI.text = entry.characterName;
            characterImageUI.sprite = entry.characterImage;

            // Stop any ongoing typing coroutine and start a new one
            if (isTyping) StopAllCoroutines();
            StartCoroutine(TypeText(entry.dialogueText));

            // Play the voice-over using the AudioManager
            if (!string.IsNullOrEmpty(entry.voiceOverName))
            {
                AudioManager.Instance.Play(entry.voiceOverName);
            }
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueTextUI.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueTextUI.text += letter;
            yield return new WaitForSeconds(0.05f); // Adjust typing speed by changing the wait time
        }

        isTyping = false;
    }

    public void NextDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;
            dialogueTextUI.text = dialogueEntries[currentDialogueIndex].dialogueText; // Show complete text
            return;
        }

        // Stop the current voice-over immediately
        DialogueEntry currentEntry = dialogueEntries[currentDialogueIndex];
        if (!string.IsNullOrEmpty(currentEntry.voiceOverName))
        {
            Debug.Log($"Stopping audio: {currentEntry.voiceOverName}");
            AudioManager.Instance.Stop(currentEntry.voiceOverName);
        }

        currentDialogueIndex++;
        if (currentDialogueIndex < dialogueEntries.Length)
        {
            DisplayDialogue();
        }
        else
        {
            // End of dialogue, hide the UI, enable player controls, and hide the cursor
            gameObject.SetActive(false);
            EnablePlayerControls();
            HideCursor();

            // Start the timer
            if (timer != null)
            {
                timer.StartTimer(); // Start the timer with a duration of 60 seconds
            }
        }
    }

    private void DisablePlayerControls()
    {
        controller.enabled = false;
        controller.enableCrouch = false;
        controller.enableJump = false;
        controller.playerCanMove = false;
        controller.cameraCanMove = false;
    }

    private void EnablePlayerControls()
    {
        controller.enabled = true;
        controller.enableCrouch = true;
        controller.enableJump = true;
        controller.playerCanMove = true;
        controller.cameraCanMove = true;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Makes the cursor visible and free to move
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Hides the cursor and locks it to the center of the screen
    }
}
