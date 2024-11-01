using UnityEngine;

public class BlurEffect : MonoBehaviour
{
    public CanvasGroup blurCanvasGroup; // Reference to the CanvasGroup to control blur visibility

    public void EnableBlur()
    {
        blurCanvasGroup.alpha = 1f;
        blurCanvasGroup.interactable = true;
        blurCanvasGroup.blocksRaycasts = true;
    }

    public void DisableBlur()
    {
        blurCanvasGroup.alpha = 0f;
        blurCanvasGroup.interactable = false;
        blurCanvasGroup.blocksRaycasts = false;
    }
}
