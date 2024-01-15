using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Shop : MonoBehaviour
{
    public Transform counter;
    public TextMeshProUGUI pouchValue;

    public List<ItemSlot> slots;
    public List<Item> itemsToDisplay;

    public AudioClip onSoldSoundEffect;
    
    private AudioSource m_audioSource;
    private bool isPouchPutDown = false;

    public UnityEvent onItemPurchase;

    private int itemPurchased;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        UpdateItemDisplay();
    }

    public void DisplayPouchValue()
    {
        pouchValue.text = GameManager.Instance.pouch.currentCurrency.ToString();
    }

    // Function to buy the currently displayed item
    public void BuyItem(int iD)
    {
        int currentPrice = GetCurrentItemPrice(iD);

        if (isPouchPutDown)
        {
            // Check if player has enough currency to buy the item
            if (GameManager.Instance.pouch.SpendCurrency(currentPrice))
            {
                itemPurchased++;

                //Update the Panel at current item
                slots[iD].itemNameText.text = $"SOLD";
                slots[iD].itemPriceText.text = string.Empty;
                slots[iD].background.color = ClearFeedback();

                itemsToDisplay[iD].Sold();
                m_audioSource.PlayOneShot(onSoldSoundEffect);

                DisplayPouchValue();
                UpdateItemBuyable();

                itemsToDisplay[iD].transform.position = counter.position;

                if (itemPurchased <= 1)
                {
                    onItemPurchase?.Invoke();
                }
            }
            else
            {
                Debug.Log("Insufficient funds to buy the item!");
            }
        }
        else
        {
            Debug.LogWarning("Put the purse on plate! ");
        }
    }
       

    // Function to update the UI with the current item's information
    private void UpdateItemDisplay()
    {
        for (int slotIndex = 0; slotIndex < slots.Count; slotIndex++)
        {
            if (!itemsToDisplay[slotIndex].isSold)
            {
                slots[slotIndex].itemNameText.text = GetItemName(slotIndex);
                slots[slotIndex].itemPriceText.text = GetCurrentItemPrice(slotIndex).ToString();
            }
        }
    }

    public void UpdateItemBuyable()
    {
        isPouchPutDown = true;

        for (int slotIndex = 0; slotIndex < slots.Count; slotIndex++)
        {
            if (!itemsToDisplay[slotIndex].isSold)
            {
                if (GameManager.Instance.pouch.currentCurrency >= itemsToDisplay[slotIndex].value)
                {
                    slots[slotIndex].background.color = ShowItemBuyable(true);
                }
                else
                {
                    slots[slotIndex].background.color = ShowItemBuyable(false);
                }
            }
        }
    }

    // Function to get the name of an item based on its index
    private string GetItemName(int index)
    {
        return itemsToDisplay[index].name;
    }

    public bool IsItemEnable()
    {
        return isPouchPutDown;
    }

    // Function to get the price of the currently displayed item
    private int GetCurrentItemPrice(int index)
    {
        return itemsToDisplay[index].value;
    }

    // Function to show feedback with a specified color
    private Color ShowItemBuyable(bool isBuyable)
    {
        if (isBuyable)
        {
            return Color.green;
        }
        return Color.red;
    }

    // Function to clear feedback
    private Color ClearFeedback()
    {
        return new Color(0f, 0f, 0f, 0.75f);
    }

    public void ClearShop()
    {
        isPouchPutDown = false;

        pouchValue.text = string.Empty;

        foreach (ItemSlot slots in slots)
        {
            slots.background.color = ClearFeedback();
        }
    }

}

[System.Serializable]
public class ItemSlot
{
    public Image background;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
}