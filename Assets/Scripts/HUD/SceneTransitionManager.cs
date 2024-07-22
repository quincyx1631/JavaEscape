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
        Debug.Log("LoadSceneAfterAnimation called.");
        doorAnimator.SetTrigger("OpenDoor");
        isAnimationComplete = false;
        StartCoroutine(WaitForAnimationToEnd(sceneName));
    }

    private IEnumerator WaitForAnimationToEnd(string sceneName)
    {
        Debug.Log("Waiting for animation to end...");
        while (!isAnimationComplete)
        {
            yield return null;
        }
        Debug.Log("Animation complete, loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // Called by Animation Event
    public void OnAnimationComplete()
    {
        Debug.Log("OnAnimationComplete called.");
        isAnimationComplete = true;
    }
}
