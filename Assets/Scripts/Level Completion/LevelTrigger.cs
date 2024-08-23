using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public GameObject levelSelectorPanel;  // Reference to the level selector UI panel

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnlockNewLevel();
            ShowLevelSelection();
            DisableMovement();
            EnableUIInput();
        }
    }

    void UnlockNewLevel()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex >= PlayerPrefs.GetInt("ReachIndex"))
        {
            PlayerPrefs.SetInt("ReachIndex", currentSceneIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();

            Debug.Log("Level unlocked: ReachIndex updated to " + (currentSceneIndex + 1));
        }
    }

    void ShowLevelSelection()
    {
        if (levelSelectorPanel != null)
        {
            levelSelectorPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("LevelTrigger: Level selector panel is not assigned.");
        }
    }

    void DisableMovement()
    {
        FirstPersonController controller = FindObjectOfType<FirstPersonController>();
        if (controller != null)
        {
            controller.enabled = false;
            Debug.Log("FirstPersonController has been disabled.");
        }
        else
        {
            Debug.LogWarning("FirstPersonController script not found.");
        }
    }

    void EnableUIInput()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
