using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractManager : MonoBehaviour
{
    public Contract[] availableContracts; // List of available contracts
    public int activeContractIndex = -1; // Index of the active contract

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
            GameManager.Instance.pouch.currentCurrency += availableContracts[activeContractIndex].reward;

            // Reset the active contract
            activeContractIndex = -1;

            // Inform the player about completing the contract
            Debug.Log("Contract Completed!");
        }
    }

    #endregion

}
