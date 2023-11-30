using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform goal;
    public Gate gate;

    public int currentCurrency = 0;

    public Contract[] availableContracts; // List of available contracts
    public int activeContractIndex = -1; // Index of the active contract

    public Transform XRRig;

    private void Awake()
    {
        Instance = this;
    }

    #region Pouch & Currency System
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

    #region Guild & Contract System
    public void InitializeContracts()
    {
        // Load contracts from the "Contracts" folder
        availableContracts = Resources.LoadAll<Contract>("Contracts");

        if (availableContracts.Length == 0)
        {
            Debug.LogError("No contracts found. Make sure to create contracts in the 'Contracts' folder.");
        }
    }

    public void StartContract()
    {
        if (activeContractIndex == -1)
        {
            // Randomly select a contract
            activeContractIndex = Random.Range(0, availableContracts.Length);

            // Inform the player about the active contract
            Debug.Log("New Contract: " + availableContracts[activeContractIndex].description);
        }
    }

    public void CompleteContract()
    {
        if (activeContractIndex != -1)
        {
            // Award currency based on the completed contract
            currentCurrency += availableContracts[activeContractIndex].reward;

            // Reset the active contract
            activeContractIndex = -1;

            // Inform the player about completing the contract
            Debug.Log("Contract Completed!");
        }
    }

    #endregion
}
