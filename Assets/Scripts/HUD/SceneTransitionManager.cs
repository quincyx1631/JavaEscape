using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager _instance;
    private bool isAnimationComplete = false;

    public static SceneTransitionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneTransitionManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("SceneTransitionManager");
                    _instance = go.AddComponent<SceneTransitionManager>();
                }
            }
            return _instance;
        }
    }

    public void LoadSceneAfterAnimation(string sceneName, Animator doorAnimator)
    {
        doorAnimator.SetTrigger("IsOpen");
        isAnimationComplete = false; // Reset flag
        StartCoroutine(WaitForAnimationToEnd(sceneName));
    }

    private IEnumerator WaitForAnimationToEnd(string sceneName)
    {
        // Wait until animation completes
        while (!isAnimationComplete)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneName); // Load the next scene
    }

    // Called by Animation Event
    public void OnAnimationComplete()
    {
        isAnimationComplete = true; // Animation is done
    }
}
