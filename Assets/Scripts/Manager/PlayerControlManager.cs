using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    public static PlayerControlManager Instance { get; private set; }

    public FirstPersonController controller;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: To persist across scenes
        }
    }

    public void DisablePlayerControls()
    {
        if (controller != null)
        {
            controller.enableCrouch = false;
            controller.enableJump = false;
            controller.playerCanMove = false;
            controller.cameraCanMove = false;
            controller.enableHeadBob = false;
            controller.enableZoom = false;
        }
    }

    public void EnablePlayerControls()
    {
        if (controller != null)
        {
            controller.enableCrouch = true;
            controller.enableJump = true;
            controller.playerCanMove = true;
            controller.cameraCanMove = true;
            controller.enableHeadBob = true;
            controller.enableZoom = true;
        }
    }
}
