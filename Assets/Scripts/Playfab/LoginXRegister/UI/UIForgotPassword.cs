using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UIForgotPassword : MonoBehaviour
{
    [SerializeField] Text feedbackText;
    [SerializeField] Canvas ForgotPassCanvas;
    [SerializeField] Canvas SigninCanvas;

    string emailAddress;

    private const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    private void OnEnable()
    {
        UserAccountManager.OnForgotPasswordSuccess.AddListener(OnForgotPasswordSuccess);
        UserAccountManager.OnForgotPasswordFailed.AddListener(OnForgotPasswordFailed);
    }

    private void OnDisable()
    {
        UserAccountManager.OnForgotPasswordSuccess.RemoveListener(OnForgotPasswordSuccess);
        UserAccountManager.OnForgotPasswordFailed.RemoveListener(OnForgotPasswordFailed);
    }

    void OnForgotPasswordFailed(string FeedbackText)
    {
        feedbackText.text = FeedbackText;
        feedbackText.gameObject.SetActive(true);
    }

    void OnForgotPasswordSuccess()
    {
        feedbackText.text = "Recovery email sent successfully!";
        feedbackText.gameObject.SetActive(true);
    }

    public void UpdateEmail(string _emailAddress)
    {
        emailAddress = _emailAddress;
    }

    public void ForgotPassword()
    {
        if (string.IsNullOrEmpty(emailAddress))
        {
            OnForgotPasswordFailed("Please enter an email address.");
            return;
        }

        if (!IsValidEmail(emailAddress))
        {
            OnForgotPasswordFailed("Please enter a valid email address.");
            return;
        }

        feedbackText.gameObject.SetActive(false);
        UserAccountManager.Instance.ForgotPassword(emailAddress);
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, EmailPattern);
    }
}
