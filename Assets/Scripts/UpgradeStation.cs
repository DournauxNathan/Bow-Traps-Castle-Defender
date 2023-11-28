using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UpgradeStation : MonoBehaviour
{
    public GameObject baseArrow;
    public Item currentUpgrade;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Item>(out Item item))
        {
            currentUpgrade = item;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Item>(out Item item))
        {
            currentUpgrade = null;
        }
    }

    public void CombineUpgrade()
    {
        // Implement logic to check the combination and create a new arrow type
        if (CheckCombination())
        {
            CreateNewArrow();
        }
    }

    private bool CheckCombination()
    {
        // Implement logic to check the combination of arrows and items in sockets
        // Return true if the combination is valid for creating a new arrow
        if (currentUpgrade.type == Item.Type.Upgrade)
        {
            switch (currentUpgrade.name)
            {
                case "Bomb":
                    return true;
                case "Fire Shard":
                    Debug.Log("You've made a Fire Arrow !");
                    return true;
                case "Electric Shard":
                    return true;
            }
        }

        return false; 
    }

    private void CreateNewArrow()
    {
        // Instantiate the explosive arrow prefab at the arrowSpawnPoint
    }
}
