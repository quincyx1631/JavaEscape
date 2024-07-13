using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance { get; private set; }

    public int Money { get; private set; }
    public TMP_Text moneyText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        Debug.Log("Money added: " + amount + ". Total Money: " + Money); // Debug log
        UpdateMoneyUI();
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "$ " + Money.ToString();
            Debug.Log("Money UI updated: " + moneyText.text); // Debug log
        }
        else
        {
            Debug.LogWarning("moneyText is not assigned.");
        }
    }
}
