using UnityEngine;

public class NumberButton : MonoBehaviour
{
    public LockerBox lockerBox;
    public int digitIndex; // 1, 2, or 3 to determine which digit to change

    public void OnButtonPress()
    {
        // Call the correct ChangeDigit method based on digitIndex
        if (lockerBox != null)
        {
            switch (digitIndex)
            {
                case 1:
                    lockerBox.ChangeDigit1();
                    break;
                case 2:
                    lockerBox.ChangeDigit2();
                    break;
                case 3:
                    lockerBox.ChangeDigit3();
                    break;
                default:
                    Debug.LogWarning("Invalid digit index assigned to NumberButton.");
                    break;
            }
        }
        else
        {
            Debug.LogWarning("LockerBox reference not set in NumberButton.");
        }
    }
}
