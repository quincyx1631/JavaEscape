using UnityEngine;
using TMPro;

public class LockerBox : MonoBehaviour
{
    // Define the TMP texts where the password will be displayed
    public TMP_Text passwordDigit1;
    public TMP_Text passwordDigit2;
    public TMP_Text passwordDigit3;

    // Define the correct password (now settable in the Inspector)
    [Header("Password Settings")]
    public string correctPassword = "123"; // Set your desired password here

    // Store the current entered password
    private string currentPassword = "";

    // Reference to the LockerBox to rotate
    public Transform lockerBoxTransform;

    // Set the desired rotation when the box is opened
    public Vector3 openRotation;

    // Initialize the TMP texts with default values
    void Start()
    {
        passwordDigit1.text = "_";
        passwordDigit2.text = "_";
        passwordDigit3.text = "_";
    }

    // Function to change the number when a button is clicked
    public void ChangeNumber(string number)
    {
        if (currentPassword.Length < 3) // Ensure we only have 3 digits
        {
            currentPassword += number;

            // Update the TMP texts based on the entered digits
            if (currentPassword.Length == 1)
                passwordDigit1.text = currentPassword[0].ToString();
            else if (currentPassword.Length == 2)
                passwordDigit2.text = currentPassword[1].ToString();
            else if (currentPassword.Length == 3)
                passwordDigit3.text = currentPassword[2].ToString();
        }

        // Check if the password is complete and matches the correct password
        if (currentPassword.Length == 3)
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
    }

    // This function opens the locker box with a custom rotation
    public void OpenBox()
    {
        Debug.Log("Password correct. Opening box...");

        // Set the rotation of the locker box to the specified open rotation
        lockerBoxTransform.rotation = Quaternion.Euler(openRotation);
    }
}
