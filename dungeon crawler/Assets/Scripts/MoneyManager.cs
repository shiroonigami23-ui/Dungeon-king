using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MoneyManager : MonoBehaviour
{
    // money counting
    private Text moneyText;
    public float moneyAmount;

    // money types
    public Currency[] Currencies;
    private int[] AllCurrencyIDs;
    private int TotalItems;

    void Awake()
    {
        moneyText = GameObject.FindGameObjectWithTag("CoinCountText").GetComponent<Text>();
        TotalItems = Currencies.Length;
        AllCurrencyIDs = new int[TotalItems];
        for (int i = 0; i < Currencies.Length; i++)
        {
            AllCurrencyIDs[i] = Currencies[i].ID;
        }
    }

    public void AddMoney(int id)
    {
        int ID = id;
        Currency i = Array.Find(Currencies, Currency => Currency.ID == ID);
        moneyAmount += i.worth;
        UpdateMoneyUI();
    }

    public void UpdateMoneyUI()
    {
        moneyText.text = "" + moneyAmount;
    }
}
