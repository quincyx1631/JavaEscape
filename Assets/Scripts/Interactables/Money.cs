using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int moneyAmount = 100;

    public void Interact()
    {
        PlayerMoney.Instance.AddMoney(moneyAmount);
        Destroy(gameObject); // Destroy the money object after pickup
    }
}
