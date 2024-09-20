using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class QuizLevelSelector : MonoBehaviour
{
    public Button[] Quizbuttons;
    public GameObject QuizWeekSelection;
    public GameObject[] quizLevels;
    private int currentQuiz;

    public GameObject floatingMessage; // Reference to the floating message GameObject
    public TextMeshProUGUI floatingMessageText; // TextMeshProUGUI component for the floating message
    public float messageDisplayDuration = 3f; // Duration for the floating message

    private void Start()
    {
        // Subscribe to the profile data updated event
        UserProfile.OnProfileDataUpdated.AddListener(UpdateQuizSelection);

        // Initialize the level buttons based on current profile data
        if (UserProfile.Instance != null)
        {
            UpdateQuizSelection(UserProfile.Instance.profileData);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the profile data updated event
        UserProfile.OnProfileDataUpdated.RemoveListener(UpdateQuizSelection);
    }

    public void UpdateQuizSelection(ProfileData profileData)
    {
        currentQuiz = profileData.level;

        // Loop through the quiz buttons and set interactability
        for (int i = 0; i < Quizbuttons.Length; i++)
        {
            // The quiz button is interactable only if the corresponding level is completed
            if (i < currentQuiz)
            {
                Quizbuttons[i].interactable = true;
            }
            else
            {
                Quizbuttons[i].interactable = false;
            }
        }
    }

    public void OpenQuizLevel(int QuizWeekNo)
    {
        string quizWeekLabel = "Level " + QuizWeekNo;

        // Disable all quiz level GameObjects
        foreach (GameObject quizLevel in quizLevels)
        {
            quizLevel.SetActive(false);
        }

        // Assuming the naming convention or some other mechanism maps the quizWeekLabel to a specific GameObject
        // Activate the selected quiz level GameObject
        quizLevels[QuizWeekNo - 1].SetActive(true); // Assuming quizWeekNo starts from 1

        // Close the level selection UI
        QuizWeekSelection.SetActive(false);
    }

    private void Update()
    {
        DetectNonInteractableButtonClick();
    }

    // Method to detect clicks on non-interactable quiz buttons
    private void DetectNonInteractableButtonClick()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                Button clickedButton = result.gameObject.GetComponent<Button>();

                if (clickedButton != null && !clickedButton.interactable)
                {
                    // Determine the quiz button's index if it's a non-interactable button
                    for (int i = 0; i < Quizbuttons.Length; i++)
                    {
                        if (Quizbuttons[i] == clickedButton && i >= currentQuiz)
                        {
                            ShowFloatingMessage("You need to finish the Corresponding Level to unlock the Quiz!");
                            break;
                        }
                    }
                }
            }
        }
    }

    // Method to show the floating message
    private void ShowFloatingMessage(string message)
    {
        floatingMessageText.text = message; // Update the TextMeshProUGUI text
        floatingMessage.SetActive(true); // Show the floating message
        StartCoroutine(HideFloatingMessageAfterDelay());
    }

    // Coroutine to hide the floating message after a delay
    private IEnumerator HideFloatingMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDisplayDuration);
        floatingMessage.SetActive(false); // Hide the message after the delay
    }
}
