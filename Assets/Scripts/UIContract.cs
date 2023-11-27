using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIContract : MonoBehaviour
{
    public Contract data;

    public TextMeshProUGUI description;
    public TextMeshProUGUI reward;

    private void Start()
    {
        // Hide the UI initially
        DisplayContract(data);
    }

    private void DisplayContract(Contract contract)
    {
        // Update the UI text with contract information
        description.text = $"Kill: {contract.targetCount} {contract.critter} critters";
        reward.text = $"\nReward: {contract.reward} coins";
    }
}
