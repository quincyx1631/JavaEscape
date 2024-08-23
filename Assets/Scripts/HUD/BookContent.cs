using TMPro;
using UnityEngine;

namespace ChristinaCreatesGames.Typography.Book
{
    public class BookContents : MonoBehaviour
    {
        [TextArea(10, 20)][SerializeField] private string content;
        [Space][SerializeField] private TMP_Text leftSide;
        [SerializeField] private TMP_Text rightSide;
        [Space][SerializeField] private TMP_Text leftPagination;
        [SerializeField] private TMP_Text rightPagination;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private string pageTurnSoundName;

        private void OnValidate()
        {
            UpdatePagination();

            if (leftSide.text == content)
                return;

            SetupContent();
        }

        private void Awake()
        {
            SetupContent();
            UpdatePagination();
        }

        private void SetupContent()
        {
            leftSide.text = content;
            rightSide.text = content;
        }

        private void UpdatePagination()
        {
            leftPagination.text = leftSide.pageToDisplay.ToString();
            rightPagination.text = rightSide.pageToDisplay.ToString();
        }

        public void PreviousPage()
        {
            if (leftSide.pageToDisplay < 1)
            {
                leftSide.pageToDisplay = 1;
                return;
            }

            if (leftSide.pageToDisplay - 2 > 1)
                leftSide.pageToDisplay -= 2;
            else
                leftSide.pageToDisplay = 1;

            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
            if (!string.IsNullOrEmpty(pageTurnSoundName))
            {
                Debug.Log("Playing page turn sound: " + pageTurnSoundName);
                AudioManager.Instance.Play(pageTurnSoundName);
            }
            UpdatePagination();
        }

        public void NextPage()
        {
            if (rightSide.pageToDisplay >= rightSide.textInfo.pageCount)
                return;

            if (leftSide.pageToDisplay >= leftSide.textInfo.pageCount - 1)
            {
                leftSide.pageToDisplay = leftSide.textInfo.pageCount - 1;
                rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
            }
            else
            {
                leftSide.pageToDisplay += 2;
                rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
            }

            if (!string.IsNullOrEmpty(pageTurnSoundName))
            {
                Debug.Log("Playing page turn sound: " + pageTurnSoundName);
                AudioManager.Instance.Play(pageTurnSoundName);
            }
            UpdatePagination();
        }

        // Add this method to update content dynamically
        public void SetContent(string newContent)
        {
            content = newContent; // Update the internal content string
            SetupContent();
            UpdatePagination();
        }

        // Add this method to clear the content
        public void ClearContent()
        {
            leftSide.text = string.Empty;
            rightSide.text = string.Empty;
            content = string.Empty;
            UpdatePagination();
        }
    }
}
