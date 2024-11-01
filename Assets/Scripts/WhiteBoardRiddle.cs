using UnityEngine;
using TMPro;

public class WhiteBoardRiddle : MonoBehaviour
{
    [Header("Answer and UI References")]
    [SerializeField] private string correctAnswer;
    [SerializeField] private TMP_InputField answerInputField;
    [SerializeField] private TMP_Text correctAnswerText;
    [SerializeField] private GameObject blackboard;
    [SerializeField] private AlertUI alertUI;
    [SerializeField] private Animator boxAnimator;

    [Header("Audio Clips")]
    [SerializeField] private string correctAnswerSound;
    [SerializeField] private string incorrectAnswerSound;
    [SerializeField] private string typingSound;
    [SerializeField] private string boxSoundEffect;

    private int previousTextLength = 0; // Track the length of text before changes

    private void Awake()
    {
        if (answerInputField != null)
        {
            answerInputField.onValueChanged.AddListener(PlayTypingSound);
        }
    }

    // Plays typing sound only if characters are added, not removed
    private void PlayTypingSound(string text)
    {
        if (!string.IsNullOrEmpty(typingSound) && text.Length > previousTextLength)
        {
            AudioManager.Instance.Play(typingSound);
        }
        previousTextLength = text.Length; // Update the previous text length
    }

    public void CheckAnswer()
    {
        if (answerInputField == null || alertUI == null)
        {
            Debug.LogWarning("Answer input field or alert UI is not assigned.");
            return;
        }

        string playerAnswer = answerInputField.text.Trim();

        if (playerAnswer.Equals(correctAnswer, System.StringComparison.OrdinalIgnoreCase))
        {
            HandleCorrectAnswer(playerAnswer);
        }
        else
        {
            HandleIncorrectAnswer();
        }

        answerInputField.text = string.Empty;
    }

    private void HandleCorrectAnswer(string playerAnswer)
    {
        Debug.Log("Correct Answer!");

        CorrectUIController.Instance?.ShowCorrectUI();

        if (correctAnswerText != null)
        {
            correctAnswerText.text = playerAnswer;
        }

        if (blackboard != null)
        {
            blackboard.tag = "Untagged";
            Debug.Log("Blackboard has been untagged.");
        }

        if (boxAnimator != null)
        {
            boxAnimator.SetTrigger("OpenLid");
            PlayBoxSoundEffect();
            Debug.Log("Box lid is opening.");
        }

        if (!string.IsNullOrEmpty(correctAnswerSound))
        {
            AudioManager.Instance.Play(correctAnswerSound);
        }
    }

    private void HandleIncorrectAnswer()
    {
        alertUI.ShowAlert("Incorrect Answer. Try again!");
        Debug.Log("Incorrect Answer. Try again!");

        if (!string.IsNullOrEmpty(incorrectAnswerSound))
        {
            AudioManager.Instance.Play(incorrectAnswerSound);
        }
    }

    private void PlayBoxSoundEffect()
    {
        if (!string.IsNullOrEmpty(boxSoundEffect))
        {
            AudioManager.Instance.Play(boxSoundEffect);
        }
    }
}
