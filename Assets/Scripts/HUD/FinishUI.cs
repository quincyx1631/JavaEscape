using UnityEngine;
using TMPro;

public class FinishUI : MonoBehaviour
{
    public static FinishUI Instance;

    public TMP_Text finalTimeText; // Reference to the TMP_Text component for displaying the final time

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisplayFinalTime(float finalTime)
    {
        PlayerControlManager.Instance.DisablePlayerControls();  
        int minutes = Mathf.FloorToInt(finalTime / 60);
        int seconds = Mathf.FloorToInt(finalTime % 60);
        finalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        Cursor.lockState = CursorLockMode.None;   
        Cursor.visible = true;
    }
}
