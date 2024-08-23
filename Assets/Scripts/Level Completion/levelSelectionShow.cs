using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelSelectionShow : MonoBehaviour
{
    [SerializeField] public GameObject levelSelectionPanel;

    void ShowLevelSelection()
    {
        if (levelSelectionPanel != null)
        {
            levelSelectionPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("LevelTrigger: Level selector panel is not assigned.");
        }
    }

    public void PressLevelSelectionPanel()
    {
        ShowLevelSelection();
        EnableUIInput();
    }

    void EnableUIInput()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMainMenuClick()
    {
        SceneManager.LoadScene("Main Menu Final");
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is valid
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes in the build settings to load.");
        }
    }
}
