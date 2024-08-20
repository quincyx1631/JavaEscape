using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreateAccount : MonoBehaviour
{
    [SerializeField] Text errorText;
    [SerializeField] Canvas canvas;

    string username, password, emailAddress;

    private void OnEnable()
    {
        UserAccountManager.OnCreateAccountFailed.AddListener(OnCreateAccountFailed);
        UserAccountManager.OnSignInSuccess.AddListener(OnSignInSuccess);
    }

    private void OnDisable()
    {
        UserAccountManager.OnCreateAccountFailed.RemoveListener(OnCreateAccountFailed);
        UserAccountManager.OnSignInSuccess.AddListener(OnSignInSuccess);
    }

    void OnCreateAccountFailed(string error)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = error;
    }

    void OnSignInSuccess()
    {
        canvas.enabled = false;
    }

    public void UpdateUsername(string _username)
    {
        username = _username;
    }

    public void UpdatePassword(string _password)
    {
        password = _password;
    }

    public void UpdateEmailAddress(string _emailAdress)
    {
        emailAddress = _emailAdress;
    }

    public void CreateAccount()
    {
        UserAccountManager.Instance.CreateAccount(username, emailAddress, password);
    }
}
