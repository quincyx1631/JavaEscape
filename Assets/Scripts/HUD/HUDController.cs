using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    public Image interactImage;
    public Image pickupImage;
    public Image inspectImage;
    public Image EscapeImage;

    private Animator interactAnimator;
    private Animator pickupAnimator;
    private Animator inspectAnimator;
    private Animator EscapeAnimator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep across scenes
            Debug.Log("HUDController instance assigned.");
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Duplicate HUDController instance destroyed.");
        }
    }

    private void Start()
    {
        interactAnimator = interactImage.GetComponent<Animator>();
        pickupAnimator = pickupImage.GetComponent<Animator>();
        inspectAnimator = inspectImage.GetComponent<Animator>();

        // Initialize the images to be off-screen
        InitializeImage(interactImage, interactAnimator);
        InitializeImage(pickupImage, pickupAnimator);
        InitializeImage(inspectImage, inspectAnimator);
    }

    private void InitializeImage(Image image, Animator animator)
    {
        if (image != null && animator != null)
        {
            image.gameObject.SetActive(false);
        }
    }

    public void EnableInteractImage()
    {
        if (interactImage != null && interactAnimator != null)
        {
            interactImage.gameObject.SetActive(true);
            interactAnimator.SetBool("IsVisible", true);
        }
    }

    public void DisableInteractImage()
    {
        if (interactImage != null && interactAnimator != null)
        {
            interactAnimator.SetBool("IsVisible", false);
        }
    }

    public void EnablePickupImage()
    {
        if (pickupImage != null && pickupAnimator != null)
        {
            pickupImage.gameObject.SetActive(true);
            pickupAnimator.SetBool("IsVisible", true);
        }
    }

    public void DisablePickupImage()
    {
        if (pickupImage != null && pickupAnimator != null)
        {
            pickupAnimator.SetBool("IsVisible", false);
        }
    }

    public void EnableInspectImage()
    {
        if (inspectImage != null && inspectAnimator != null)
        {
            inspectImage.gameObject.SetActive(true);
            inspectAnimator.SetBool("IsVisible", true);
        }
    }

    public void DisableInspectImage()
    {
        if (inspectImage != null && inspectAnimator != null)
        {
            inspectAnimator.SetBool("IsVisible", false);
        }
    }
    public void EnableEscapeImage()
    {
        if (EscapeImage != null && EscapeAnimator != null)
        {
            inspectImage.gameObject.SetActive(true);
           EscapeAnimator.SetBool("IsVisible", true);
        }
    }

    public void DisableEscapeImage()
    {
        if (EscapeImage != null && EscapeAnimator != null)
        {
            EscapeAnimator.SetBool("IsVisible", false);
        }
    }
}
