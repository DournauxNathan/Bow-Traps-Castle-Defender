using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UpgradeStation : MonoBehaviour
{
    [Header("REFS")]
    public GameObject baseArrow;
    public Transform newArrow;

    public List<GameObject> upgrades;
    [Header("PROPERTIES")]
    public int maxHit = 3;
    private int hitCount = 0;

    [Header("SFX")]
    public AudioClip upgradeSoundEffect;

    private Item currentUpgrade;
    private AudioSource m_AudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Item>(out Item item))
        {
            currentUpgrade = item;
            maxHit = item.hitCount;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Item>(out Item item))
        {
            maxHit = 3;
            currentUpgrade = null;
        }
    }

    public void OnHit()
    {
        hitCount++;

        if (hitCount >= maxHit)
        {
            CombineUpgrade();
        }
    }

    private void CombineUpgrade()
    {
        // Check the combination and create a new arrow type
        if (hitCount >= maxHit && CheckCombination())
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
