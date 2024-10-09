using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Net.Mail;
using UnityEngine.Events;
using System;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager Instance;

    public static UnityEvent OnSignInSuccess = new UnityEvent();

    public static UnityEvent<string> OnSignInFailed = new UnityEvent<string>();

    public static UnityEvent<string> OnCreateAccountFailed = new UnityEvent<string>();

    public static UnityEvent<string, string> OnUserDataRecieved = new UnityEvent<string, string>();

    public static UnityEvent<string> OnForgotPasswordFailed = new UnityEvent<string>();

    public static UnityEvent OnForgotPasswordSuccess = new UnityEvent();

    string playfabID;

    void Awake()
    {
        Instance = this;
    }

    public void CreateAccount(string username, string emailAddress, string password)
    {
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest()
            {
                Email = emailAddress,
                Password = password,
                Username = username,
                RequireBothUsernameAndEmail = true
            },
            response =>
            {
                Debug.Log($"Successful Account Creation: {username}, {emailAddress}");
                playfabID = response.PlayFabId;
                SignIn(username, password );
            },
            error =>
            {
                Debug.Log($"Unsuccessful Account Creation: {username}, {emailAddress} \n {error.ErrorMessage}");
                OnCreateAccountFailed.Invoke(error.ErrorMessage);
            }
        );
    }

    public void SignIn(string username, string password)
    {
        
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest ()
        {
            Username = username,
            Password = password
        },
        response =>
        {
            Debug.Log($"Successful Account Login: {username}");
            playfabID = response.PlayFabId;
            OnSignInSuccess.Invoke();
        },
        error =>
        {
            Debug.Log($"Unsuccessful Account Login: {username} \n {error.ErrorMessage}");
            OnSignInFailed.Invoke(error.ErrorMessage);
        }
        );
    }

    public void GetUserData(string key)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = playfabID,
            Keys = new List<string>() { 
                key 
            }
        },
        response => {
            Debug.Log($"Successful GetUserData");
            if(response.Data.ContainsKey(key)) OnUserDataRecieved.Invoke(key, response.Data[key].Value);
            else OnUserDataRecieved.Invoke(key, null);
        },
        error => {
            Debug.Log($"Unsuccessful GetUserData {error.ErrorMessage}");
        });
    }

    public void SetUserData(string key, string value, UnityAction OnSucess = null)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string ,string>()
            {
                { key, value }
            }
        },
        response => {
            Debug.Log($"Successful SetUserData");
            OnSucess?.Invoke();
        }, 
        error => {
            Debug.Log($"Unsuccessful SetUserData {error.ErrorMessage}");
        });
    }

    public void SetQuizData(string key, string value, UnityAction OnSucess = null)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { key, value }
            }
        },
        response => {
            Debug.Log($"Successful SetUserData");
            OnSucess?.Invoke();
        },
        error => {
            Debug.Log($"Unsuccessful SetUserData {error.ErrorMessage}");
        });
    }

    public void SetDisplayName(string displayName, Action<bool> callback)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName
        },
        result =>
        {
            Debug.Log("Display name set successfully.");
            callback?.Invoke(true); // Indicate success
        },
        error =>
        {
            Debug.LogError($"Error setting display name: {error.GenerateErrorReport()}");
            callback?.Invoke(false); // Indicate failure
        });
    }

    public void ForgotPassword(string emailAddress)
    {
        if (string.IsNullOrEmpty(emailAddress))
        {
            OnForgotPasswordFailed.Invoke("Email address is required.");
            return;
        }

        try
        {
            PlayFabClientAPI.SendAccountRecoveryEmail(new SendAccountRecoveryEmailRequest
            {
                Email = emailAddress,
                TitleId = PlayFabSettings.TitleId
            },
            response =>
            {
                Debug.Log($"Password recovery email sent successfully to {emailAddress}");
                OnForgotPasswordSuccess.Invoke();
            },
            error =>
            {
                string errorMessage = "An error occurred. Please try again.";
                if (error.Error == PlayFabErrorCode.InvalidEmailAddress ||
                    error.ErrorMessage.Contains("Invalid email address"))
                {
                    errorMessage = "This email address is not registered.";
                }
                Debug.LogWarning($"Failed to send password recovery email: {error.GenerateErrorReport()}");
                OnForgotPasswordFailed.Invoke(errorMessage);
            });
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unexpected error in ForgotPassword: {ex.Message}");
            OnForgotPasswordFailed.Invoke("An unexpected error occurred. Please try again later.");
        }
    }
}
