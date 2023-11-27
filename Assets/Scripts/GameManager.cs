using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform goal;
    public Gate gate;

    public int currentCurrency = 0;

    private void Awake()
    {
        Instance = this;
    }

    #region Pouch System
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


    #endregion
}
