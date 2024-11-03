using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TVSlideShow : MonoBehaviour
{
    public Image displayImage;
    public Slide[] slides;                // Array of Slide objects
    public Transform itemHolder;
    public GameObject remote;
    public KeypadNumbers keypadNumbers;
    public AlertUI alertUI;
    public GameObject passwordClueDisplay;
    public TextMeshProUGUI passwordText;
    public string turnOnSoundName;
    public string changeSlideSoundName;

    private int currentRandomIndex = 0;
    private bool isTVOn = false;
    private Coroutine fadeCoroutine;
    private int[] randomIndices;
    private bool[] hasShown;

    private void Start()
    {
        SetImageAlpha(0f);
        InitializeRandomOrder();
        passwordClueDisplay.SetActive(false);
    }

    private void InitializeRandomOrder()
    {
        randomIndices = new int[slides.Length];
        hasShown = new bool[slides.Length];

        for (int i = 0; i < slides.Length; i++)
        {
            randomIndices[i] = i;
            hasShown[i] = false;
        }
        ShuffleSlides();
    }

    private void ShuffleSlides()
    {
        for (int i = 0; i < randomIndices.Length; i++)
        {
            int randomIndex = Random.Range(0, randomIndices.Length);
            int temp = randomIndices[i];
            randomIndices[i] = randomIndices[randomIndex];
            randomIndices[randomIndex] = temp;
        }
        currentRandomIndex = 0;
    }

    private void TurnOnTV()
    {
        isTVOn = true;
        AudioManager.Instance.Play(turnOnSoundName);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeInTV());
    }

    private IEnumerator FadeInTV()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            SetImageAlpha(alpha);
            yield return null;
        }
        SetImageAlpha(1f);
        ShowNextSlide();
    }

    public void ShowNextSlide()
    {
        if (slides.Length == 0) return;

        hasShown[randomIndices[currentRandomIndex]] = true;
        AudioManager.Instance.Play(changeSlideSoundName);

        currentRandomIndex++;
        if (currentRandomIndex >= randomIndices.Length)
        {
            InitializeRandomOrder();
            return;
        }

        while (hasShown[randomIndices[currentRandomIndex]])
        {
            currentRandomIndex++;
            if (currentRandomIndex >= randomIndices.Length)
            {
                InitializeRandomOrder();
                return;
            }
        }

        UpdateSlideImage();
    }

    public void ShowPasswordClue()
    {
        passwordClueDisplay.SetActive(true);
        GenerateRandomPassword();

        if (keypadNumbers != null)
        {
            keypadNumbers.SetKeypadPassword(passwordText.text);
        }
        else
        {
            Debug.LogError("KeypadNumbers reference is missing.");
        }
    }

    private void GenerateRandomPassword()
    {
        int password = Random.Range(10000, 99999);
        passwordText.text = password.ToString();
    }

    private void UpdateSlideImage()
    {
        int slideIndex = randomIndices[currentRandomIndex];
        displayImage.sprite = slides[slideIndex].slideImage; // Using slideImage from Slide
    }

    private bool HasRemote()
    {
        return itemHolder.childCount > 0 && itemHolder.GetChild(0).gameObject == remote;
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = displayImage.color;
        color.a = alpha;
        displayImage.color = color;
    }

    public void InteractWithTV()
    {
        if (!isTVOn)
        {
            if (HasRemote())
            {
                TurnOnTV();
            }
            else
            {
                alertUI.ShowAlert("You need a remote to turn on the TV.");
            }
        }
        else
        {
            if (HasRemote())
            {
                ShowNextSlide();
            }
            else
            {
                alertUI.ShowAlert("You need a remote to change the slide.");
            }
        }
    }

    public string GetCurrentSlideTitle()
    {
        return slides[randomIndices[currentRandomIndex]].slideTitle; // Return the slide title from Slide
    }

    public bool IsTVOn()
    {
        return isTVOn;
    }
}
