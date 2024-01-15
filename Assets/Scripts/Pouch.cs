using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouch : MonoBehaviour
{
    public int currentCurrency = 0;

    public void Start()
    {
        GameManager.Instance?.GetPouchInfo(this);
    }

    public void AddCurency(int amount)
    {
        currentCurrency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currentCurrency)
        {
            currentCurrency -= amount;
            return true; // Successfully spent currency
        }
        else
        {
            return false; // Insufficient funds
        }
    }
}