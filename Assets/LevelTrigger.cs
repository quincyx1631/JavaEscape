using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnlockNewLevel();
            LoadLevelSelection();
        }
    }

    void UnlockNewLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex >= PlayerPrefs.GetInt("ReachIndex"))
        {
            PlayerPrefs.SetInt("ReachIndex", currentSceneIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();

            Debug.Log("Level unlocked: ReachIndex updated to " + (currentSceneIndex + 1));
        }
    }

    void LoadLevelSelection()
    {
        // Load the level selection scene
        SceneManager.LoadScene("Main Menu Final");
    }
}
