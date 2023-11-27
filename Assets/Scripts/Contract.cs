using UnityEngine;

[CreateAssetMenu(fileName = "NewContract", menuName = "Contracts/New Contract")]
public class Contract : ScriptableObject
{
    [Header("Contract Details")]
    public string description;
    public CritterType critter;
    public int targetCount; // Number of critters to defeat to complete the contract
    public int reward; // Currency reward for completing the contract
}