using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class QuizLeaderboard : MonoBehaviour

{
    int leaderboardID = 24071;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID.ToString(), (response) =>
        {
            if (response.success)
            {
                Debug.Log("Succesfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.errorData);
                done = true;
            }
        });
        yield return new WaitWhile(()=> done == false);
    }
}
