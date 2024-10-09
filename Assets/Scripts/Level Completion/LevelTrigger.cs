using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public GameObject levelSelectorPanel;  // Reference to the level selector UI panel

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {

            // Show the level selection UI and update the UI inputs
            ShowLevelSelection();
            DisableMovement();
            EnableUIInput();

            // Refresh the level selection buttons
            RefreshLevelSelection();
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
        Debug.Log("Cursor unlocked and visible.");
    }

    void RefreshLevelSelection()
    {
        if (levelSelectorPanel != null)
        {
            LevelSelector levelSelector = levelSelectorPanel.GetComponent<LevelSelector>();
            if (levelSelector != null)
            {
                // Refresh the buttons based on the latest profile data
                levelSelector.UpdateLevelSelection(UserProfile.Instance.profileData);
            }
            else
            {
                Debug.LogError("LevelTrigger: LevelSelector script not found on the level selector panel.");
            }
        }
    }
}
