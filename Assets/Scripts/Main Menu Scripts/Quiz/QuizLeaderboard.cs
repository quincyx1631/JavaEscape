using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class QuizLeaderboard : MonoBehaviour
{
    public TMP_InputField studentSectionInputField;
    public TMP_InputField studentNameInputField;
    int leaderboardID = 24125;
    string leaderboardKey = "quizRoom1";

    void Start()
    {
        studentSectionInputField.onEndEdit.AddListener(delegate { OnSubmitMetadata(0); });
        studentNameInputField.onEndEdit.AddListener(delegate { OnSubmitMetadata(0); });
    }

    public void OnSubmitMetadata(int score)
    {
        string studName = studentNameInputField.text;
        string studSection = studentSectionInputField.text;

        string playerID = PlayerPrefs.GetString("PlayerID");
        string studScore = score.ToString();
        // Create metadata as a plain string without curly braces
        string metadata = "Student Name: " + studName + ", Student Section: " + studSection;

        // Submit the score with metadata
        StartCoroutine(SubmitScoreWithMetadataRoutine(playerID, score, metadata));
    }

    public IEnumerator SubmitScoreWithMetadataRoutine(string playerID, int score, string metadata)
    {
        bool done = false;

        LootLockerSDKManager.SubmitScore(playerID, score, leaderboardKey, metadata, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score with metadata!");
                done = true;
            }
            else
            {
                Debug.Log("Failed to upload score with metadata: " + response.errorData);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(studentNameInputField.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Succesfully set player name");
            }
            else
            {
                Debug.Log("Could not set player name" + response.errorData);

            }
        });
    }
}