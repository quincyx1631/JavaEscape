using UnityEngine;
using TMPro;

public class LockerBox : MonoBehaviour
{
    public TMP_Text passwordDigit1;
    public TMP_Text passwordDigit2;
    public TMP_Text passwordDigit3;

    [Header("Password Settings")]
    public string correctPassword = "123";

    private string currentPassword = "000";

    public Animator lockerAnimator;
    public Transform lockerBoxTransform;
    public Vector3 openRotation;

    // Array to hold the button GameObjects
    public GameObject[] numberButtons;

    void Start()
    {
        UpdatePasswordDisplay();
    }

    public void ChangeDigit1()
    {
        int digit = (int.Parse(passwordDigit1.text) + 1) % 10;
        passwordDigit1.text = digit.ToString();
        UpdateCurrentPassword();
    }

    public void ChangeDigit2()
    {
        int digit = (int.Parse(passwordDigit2.text) + 1) % 10;
        passwordDigit2.text = digit.ToString();
        UpdateCurrentPassword();
    }

    public void ChangeDigit3()
    {
        int digit = (int.Parse(passwordDigit3.text) + 1) % 10;
        passwordDigit3.text = digit.ToString();
        UpdateCurrentPassword();
    }

    private void UpdateCurrentPassword()
    {
        currentPassword = passwordDigit1.text + passwordDigit2.text + passwordDigit3.text;
        CheckPassword();
    }

    private void CheckPassword()
    {
        if (currentPassword == correctPassword)
        {
            OpenBox();
        }
        else
        {
            Debug.Log("Incorrect password.");
        }
    }

    public void OpenBox()
    {
        Debug.Log("Password correct. Opening box...");
        lockerAnimator.SetTrigger("OpenBox"); // Trigger the animation

        // Set each button's tag to "Untagged" after the box is opened
        foreach (GameObject button in numberButtons)
        {
            button.tag = "Untagged";
        }
    }

    private void UpdatePasswordDisplay()
    {
        passwordDigit1.text = "0";
        passwordDigit2.text = "0";
        passwordDigit3.text = "0";
    }
}
