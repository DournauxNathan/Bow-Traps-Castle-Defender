using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UpgradeStation : MonoBehaviour
{
    public GameObject baseArrow;
    public Transform newArrow;

    private Item currentUpgrade;

    public List<GameObject> upgrades;

    private int hitCount = 0;

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
        hitCount++;

        // Implement logic to check the combination and create a new arrow type
        if (CheckCombination() && hitCount >= 3)
        {
            hitCount = 0;
            CreateNewArrow();
        }
    }

    private bool CheckCombination()
    {
        if (currentUpgrade.type == Item.Type.Upgrade)
        {
            switch (currentUpgrade.name)
            {
                case "Bomb":
                    SetNewArrowPosition("Explosive Arrow");
                    Debug.Log("You've made a Explosive Arrow !");
                    break;
                case "Fire Shard":
                    SetNewArrowPosition("Fire Arrow");
                    Debug.Log("You've made a Fire Arrow !");
                    break;
                case "Electric Shard":
                    SetNewArrowPosition("Lightning Arrow");
                    Debug.Log("You've made a Lightning Arrow !");
                    break;
            }
            return true;
        }

        return false; 
    }

    private void CreateNewArrow()
    {
        baseArrow.SetActive(false);
        currentUpgrade.gameObject.SetActive(false);
        // Instantiate the explosive arrow prefab at the arrowSpawnPoint
    }

    public void SetNewArrowPosition(string arrowName)
    {
        // Your logic when the arrow is in the list
        GameObject foundArrow = upgrades.Find(a => a.name == arrowName);

        Debug.Log("", foundArrow);

        if (foundArrow != null)
        {
            foundArrow.SetActive(true);
            // Do something with the found arrow, for example, set its position
            foundArrow.transform.position = newArrow.position;
        }
        else
        {
            Debug.Log("Arrow was not found");
        }
    }
}
