using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string characterName;
        public Sprite characterImage;
        [TextArea]
        public string dialogueText;
        public string voiceOverName; // Name of the voice-over in the AudioManager
    }

    public Image characterImageUI;
    public TMP_Text characterNameUI;
    public TMP_Text dialogueTextUI;
    public Button nextButton;
    public Timer timer; // Reference to the Timer script or component

    public DialogueEntry[] dialogueEntries;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    public FadeInIntro fadeInIntro;
    public TVTutsPlayer tv;
    private void Start()
    {
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(NextDialogue);
        }

        PlayerControlManager.Instance.DisablePlayerControls();
        MouseManager.Instance.EnableMouse();

        StartDialogue();
    }

    public void StartDialogue()
    {
        currentDialogueIndex = 0; // Reset index
        DisplayDialogue();
        PauseMenuController.Instance.disableTab();
    }

    private void DisplayDialogue()
    {
        if (currentDialogueIndex >= 0 && currentDialogueIndex < dialogueEntries.Length)
        {
            DialogueEntry entry = dialogueEntries[currentDialogueIndex];
            characterNameUI.text = entry.characterName;
            characterImageUI.sprite = entry.characterImage;

            // Reset dialogue text and stop any ongoing typing coroutine
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            // Clear the text before typing animation starts
            dialogueTextUI.text = "";

            // Start typing animation with a small enforced delay
            typingCoroutine = StartCoroutine(TypeText(entry.dialogueText));

            // Play the voice-over using the AudioManager
            if (!string.IsNullOrEmpty(entry.voiceOverName) && AudioManager.Instance != null)
            {
                AudioManager.Instance.StopAll(); // Stop any currently playing audio
                AudioManager.Instance.Play(entry.voiceOverName);
            }
        }
        else
        {
            EndDialogue();
        }
    }



    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueTextUI.text = ""; // Ensure text starts blank each time

        // Adding a small delay at the beginning to enforce reset
        yield return new WaitForSeconds(0.1f);

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
            // If typing animation is still playing, stop it and complete the text
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            isTyping = false;
            dialogueTextUI.text = dialogueEntries[currentDialogueIndex].dialogueText; // Complete the current text
            return; // Require another click to move to the next dialogue
        }

        // Stop any ongoing voice-over for the current dialogue
        if (currentDialogueIndex >= 0 && currentDialogueIndex < dialogueEntries.Length)
        {
            DialogueEntry currentEntry = dialogueEntries[currentDialogueIndex];
            if (!string.IsNullOrEmpty(currentEntry.voiceOverName) && AudioManager.Instance != null)
            {
                AudioManager.Instance.Stop(currentEntry.voiceOverName);
            }
        }

        // Proceed to the next dialogue
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogueEntries.Length)
        {
            DisplayDialogue(); // Show the next dialogue
        }
        else
        {
            EndDialogue(); // End dialogue sequence
        }
    }


    private void EndDialogue()
    {
        // End of dialogue, hide the UI, enable player controls, and hide the cursor
        gameObject.SetActive(false);
        PlayerControlManager.Instance.EnablePlayerControls();
        MouseManager.Instance.DisableMouse();

        // Start the timer
        if (timer != null)
        {
            timer.StartTimer(); // Start the timer with a duration of 60 seconds
        }
        if (tv != null)
        {

        tv.PlayTVVideo();
        
        }
        if (fadeInIntro != null)
        {
            fadeInIntro.ShowUIElements();
            
        }
    }
}
