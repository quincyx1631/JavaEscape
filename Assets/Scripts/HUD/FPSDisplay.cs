using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;  // Reference to the UI Text component
    public float updateInterval = 0.5f;  // Time interval to average the FPS
    private float timeElapsed = 0.0f;
    private int frameCount = 0;
    private float fps = 0.0f;

    void Update()
    {
        timeElapsed += Time.unscaledDeltaTime;
        frameCount++;

        // Update FPS every 'updateInterval' seconds
        if (timeElapsed >= updateInterval)
        {
            fps = frameCount / timeElapsed;
            fpsText.text = $"FPS: {Mathf.Ceil(fps)}";

            // Reset counters
            timeElapsed = 0.0f;
            frameCount = 0;
        }
    }
}
