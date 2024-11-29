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
                SignIn(username, password);
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

        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
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
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = playfabID,
            Keys = new List<string>() {
                key
            }
        },
        response =>
        {
            Debug.Log($"Successful GetUserData");
            if (response.Data.ContainsKey(key)) OnUserDataRecieved.Invoke(key, response.Data[key].Value);
            else OnUserDataRecieved.Invoke(key, null);
        },
        error =>
        {
            Debug.Log($"Unsuccessful GetUserData {error.ErrorMessage}");
        });
    }

    public void SetUserData(string key, string value, UnityAction OnSucess = null)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { key, value }
            }
        },
        response =>
        {
            Debug.Log($"Successful SetUserData");
            OnSucess?.Invoke();
        },
        error =>
        {
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
        response =>
        {
            Debug.Log($"Successful SetUserData");
            OnSucess?.Invoke();
        },
        error =>
        {
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
    public void SubmitQuiz1ScoreToLeaderboard()
    {
        if (string.IsNullOrEmpty(playfabID))
        {
            Debug.LogWarning("PlayFab not logged in. Cannot submit leaderboard score.");
            return;
        }

        UserProfile userProfile = UserProfile.Instance;
        if (!string.IsNullOrEmpty(userProfile.quizData.QuizNumber1))
        {
            int score = int.Parse(userProfile.quizData.QuizNumber1.Split('/')[0]);

            string statisticName = userProfile.profileData.Student_Section == "BSIT 2A"
                ? "Quiz 1 Score - BSIT 2A"
                : "Quiz 1 Score - BSIT 2B";

            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                    new StatisticUpdate
                    {
                        StatisticName = statisticName,
                        Value = score
                    }
                    }
                },
                result =>
                {
                    Debug.Log($"Successfully submitted Quiz 1 score to {userProfile.profileData.Student_Section} leaderboard");
                },
                error =>
                {
                    Debug.LogError($"Failed to submit Quiz 1 score: {error.ErrorMessage}");
                }
            );
        }
        else
        {
            Debug.LogWarning("Cannot submit leaderboard score: No Quiz 1 score found.");
        }
    }

    public void SubmitQuiz2ScoreToLeaderboard()
    {
        if (string.IsNullOrEmpty(playfabID))
        {
            Debug.LogWarning("PlayFab not logged in. Cannot submit leaderboard score.");
            return;
        }

        UserProfile userProfile = UserProfile.Instance;
        if (!string.IsNullOrEmpty(userProfile.quizData.QuizNumber2))
        {
            int score = int.Parse(userProfile.quizData.QuizNumber2.Split('/')[0]);

            string statisticName = userProfile.profileData.Student_Section == "BSIT 2A"
                ? "Quiz 2 Score - BSIT 2A"
                : "Quiz 2 Score - BSIT 2B";

            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                    new StatisticUpdate
                    {
                        StatisticName = statisticName,
                        Value = score
                    }
                    }
                },
                result =>
                {
                    Debug.Log($"Successfully submitted Quiz 2 score to {userProfile.profileData.Student_Section} leaderboard");
                },
                error =>
                {
                    Debug.LogError($"Failed to submit Quiz 2 score: {error.ErrorMessage}");
                }
            );
        }
        else
        {
            Debug.LogWarning("Cannot submit leaderboard score: No Quiz 2 score found.");
        }
    }

    public void SubmitQuiz3ScoreToLeaderboard()
    {
        if (string.IsNullOrEmpty(playfabID))
        {
            Debug.LogWarning("PlayFab not logged in. Cannot submit leaderboard score.");
            return;
        }

        UserProfile userProfile = UserProfile.Instance;
        if (!string.IsNullOrEmpty(userProfile.quizData.QuizNumber3))
        {
            int score = int.Parse(userProfile.quizData.QuizNumber3.Split('/')[0]);

            string statisticName = userProfile.profileData.Student_Section == "BSIT 2A"
                ? "Quiz 3 Score - BSIT 2A"
                : "Quiz 3 Score - BSIT 2B";

            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                    new StatisticUpdate
                    {
                        StatisticName = statisticName,
                        Value = score
                    }
                    }
                },
                result =>
                {
                    Debug.Log($"Successfully submitted Quiz 3 score to {userProfile.profileData.Student_Section} leaderboard");
                },
                error =>
                {
                    Debug.LogError($"Failed to submit Quiz 3 score: {error.ErrorMessage}");
                }
            );
        }
        else
        {
            Debug.LogWarning("Cannot submit leaderboard score: No Quiz 3 score found.");
        }
    }

    public void SubmitQuiz4ScoreToLeaderboard()
    {
        if (string.IsNullOrEmpty(playfabID))
        {
            Debug.LogWarning("PlayFab not logged in. Cannot submit leaderboard score.");
            return;
        }

        UserProfile userProfile = UserProfile.Instance;
        if (!string.IsNullOrEmpty(userProfile.quizData.QuizNumber4))
        {
            int score = int.Parse(userProfile.quizData.QuizNumber4.Split('/')[0]);

            string statisticName = userProfile.profileData.Student_Section == "BSIT 2A"
                ? "Quiz 4 Score - BSIT 2A"
                : "Quiz 4 Score - BSIT 2B";

            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                    new StatisticUpdate
                    {
                        StatisticName = statisticName,
                        Value = score
                    }
                    }
                },
                result =>
                {
                    Debug.Log($"Successfully submitted Quiz 4 score to {userProfile.profileData.Student_Section} leaderboard");
                },
                error =>
                {
                    Debug.LogError($"Failed to submit Quiz 4 score: {error.ErrorMessage}");
                }
            );
        }
        else
        {
            Debug.LogWarning("Cannot submit leaderboard score: No Quiz 4 score found.");
        }
    }

    public void SubmitQuiz5ScoreToLeaderboard()
    {
        if (string.IsNullOrEmpty(playfabID))
        {
            Debug.LogWarning("PlayFab not logged in. Cannot submit leaderboard score.");
            return;
        }

        UserProfile userProfile = UserProfile.Instance;
        if (!string.IsNullOrEmpty(userProfile.quizData.QuizNumber5))
        {
            int score = int.Parse(userProfile.quizData.QuizNumber5.Split('/')[0]);

            string statisticName = userProfile.profileData.Student_Section == "BSIT 2A"
                ? "Quiz 5 Score - BSIT 2A"
                : "Quiz 5 Score - BSIT 2B";

            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                    new StatisticUpdate
                    {
                        StatisticName = statisticName,
                        Value = score
                    }
                    }
                },
                result =>
                {
                    Debug.Log($"Successfully submitted Quiz 5 score to {userProfile.profileData.Student_Section} leaderboard");
                },
                error =>
                {
                    Debug.LogError($"Failed to submit Quiz 5 score: {error.ErrorMessage}");
                }
            );
        }
        else
        {
            Debug.LogWarning("Cannot submit leaderboard score: No Quiz 5 score found.");
        }
    }

    public void SubmitQuiz6ScoreToLeaderboard()
    {
        if (string.IsNullOrEmpty(playfabID))
        {
            Debug.LogWarning("PlayFab not logged in. Cannot submit leaderboard score.");
            return;
        }

        UserProfile userProfile = UserProfile.Instance;
        if (!string.IsNullOrEmpty(userProfile.quizData.QuizNumber6))
        {
            int score = int.Parse(userProfile.quizData.QuizNumber6.Split('/')[0]);

            string statisticName = userProfile.profileData.Student_Section == "BSIT 2A"
                ? "Quiz 6 Score - BSIT 2A"
                : "Quiz 6 Score - BSIT 2B";

            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                    new StatisticUpdate
                    {
                        StatisticName = statisticName,
                        Value = score
                    }
                    }
                },
                result =>
                {
                    Debug.Log($"Successfully submitted Quiz 6 score to {userProfile.profileData.Student_Section} leaderboard");
                },
                error =>
                {
                    Debug.LogError($"Failed to submit Quiz 6 score: {error.ErrorMessage}");
                }
            );
        }
        else
        {
            Debug.LogWarning("Cannot submit leaderboard score: No Quiz 6 score found.");
        }
    }

    public void SubmitQuiz7ScoreToLeaderboard()
    {
        if (string.IsNullOrEmpty(playfabID))
        {
            Debug.LogWarning("PlayFab not logged in. Cannot submit leaderboard score.");
            return;
        }

        UserProfile userProfile = UserProfile.Instance;
        if (!string.IsNullOrEmpty(userProfile.quizData.QuizNumber7))
        {
            int score = int.Parse(userProfile.quizData.QuizNumber7.Split('/')[0]);

            string statisticName = userProfile.profileData.Student_Section == "BSIT 2A"
                ? "Quiz 7 Score - BSIT 2A"
                : "Quiz 7 Score - BSIT 2B";

            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                    new StatisticUpdate
                    {
                        StatisticName = statisticName,
                        Value = score
                    }
                    }
                },
                result =>
                {
                    Debug.Log($"Successfully submitted Quiz 7 score to {userProfile.profileData.Student_Section} leaderboard");
                },
                error =>
                {
                    Debug.LogError($"Failed to submit Quiz 7 score: {error.ErrorMessage}");
                }
            );
        }
        else
        {
            Debug.LogWarning("Cannot submit leaderboard score: No Quiz 7 score found.");
        }
    }

    public void SubmitQuiz8ScoreToLeaderboard()
    {
        if (string.IsNullOrEmpty(playfabID))
        {
            Debug.LogWarning("PlayFab not logged in. Cannot submit leaderboard score.");
            return;
        }

        UserProfile userProfile = UserProfile.Instance;
        if (!string.IsNullOrEmpty(userProfile.quizData.QuizNumber8))
        {
            int score = int.Parse(userProfile.quizData.QuizNumber8.Split('/')[0]);

            string statisticName = userProfile.profileData.Student_Section == "BSIT 2A"
                ? "Quiz 8 Score - BSIT 2A"
                : "Quiz 8 Score - BSIT 2B";

            PlayFabClientAPI.UpdatePlayerStatistics(
                new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                    new StatisticUpdate
                    {
                        StatisticName = statisticName,
                        Value = score
                    }
                    }
                },
                result =>
                {
                    Debug.Log($"Successfully submitted Quiz 8 score to {userProfile.profileData.Student_Section} leaderboard");
                },
                error =>
                {
                    Debug.LogError($"Failed to submit Quiz 8 score: {error.ErrorMessage}");
                }
            );
        }
        else
        {
            Debug.LogWarning("Cannot submit leaderboard score: No Quiz 8 score found.");
        }
    }
}
