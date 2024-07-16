using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    public TextMeshProUGUI interactText;
    public TextMeshProUGUI pickupText;
    public TextMeshProUGUI inspectText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnableInteractionText(string message)
    {
        interactText.text = message;
        interactText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactText.gameObject.SetActive(false);
    }

    public void EnablePickupText()
    {
        pickupText.gameObject.SetActive(true);
    }

    public void DisablePickupText()
    {
        pickupText.gameObject.SetActive(false);
    }

    public void EnableInspectText()
    {
        inspectText.gameObject.SetActive(true);
    }

    public void DisableInspectText()
    {
        inspectText.gameObject.SetActive(false);
    }
}
