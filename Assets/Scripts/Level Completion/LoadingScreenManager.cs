// LoadingScreenManager.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private float minLoadTime = 7f;

    private static LoadingScreenManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static LoadingScreenManager Instance => instance;

    public void StartLoading()
    {
        StartCoroutine(LoadSceneAsync("Main Menu Final"));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Show loading screen and reset slider
        loadingScreenPanel.SetActive(true);
        progressSlider.value = 0f;

        // Start loading the scene in background
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        float elapsedTime = 0f;
        float progress = 0f;

        // Wait for both the minimum time AND scene loading to complete
        while (elapsedTime < minLoadTime || progress < 0.9f)
        {
            elapsedTime += Time.deltaTime;
            progress = asyncOperation.progress / 0.9f;

            float visualProgress = Mathf.Min(elapsedTime / minLoadTime, progress);
            progressSlider.value = visualProgress;

            yield return null;
        }

        // Clean up DontDestroyOnLoad objects INCLUDING this one
        GameObject[] rootObjects = GameObject.FindObjectsOfType<GameObject>(true)
            .Where(go => go.scene.buildIndex == -1 && go.transform.parent == null)
            .ToArray();

        // Allow scene activation
        asyncOperation.allowSceneActivation = true;

        // Destroy everything except PlayFabHttp
        foreach (GameObject obj in rootObjects)
        {
            if (!obj.name.Contains("PlayFabHttp"))
            {
                Destroy(obj);
            }
        }
    }
}