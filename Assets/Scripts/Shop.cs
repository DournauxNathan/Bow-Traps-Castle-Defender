using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public List<ItemSlot> slots;
    public List<Item> itemsToDisplay;

    private int[] itemPrices = { 50, 100, 150 }; // Prices for the three items

    private int currentItemIndex = 0; // Index to track the current displayed item

    void Start()
    {
        UpdateItemDisplay();
    }

    // Function to buy the currently displayed item
    public void BuyItem()
    {
        int currentPrice = GetCurrentItemPrice(0);

        // Check if player has enough currency to buy the item
        if (GameManager.Instance.SpendCurrency(currentPrice))
        {
            // Implement logic to apply the purchased item's effect or upgrade
            // ...

            // Move to the next item
            currentItemIndex = (currentItemIndex + 1) % itemPrices.Length;

            // Update the item display
            UpdateItemDisplay();
        }
        else
        {
            Debug.Log("Insufficient funds to buy the item!");
        }
    }

    // Function to get the price of the currently displayed item
    private int GetCurrentItemPrice(int index)
    {
        return itemsToDisplay[index].value;
    }

    // Function to update the UI with the current item's information
    private void UpdateItemDisplay()
    {
        for (int slotIndex = 0; slotIndex < slots.Count; slotIndex++)
        {
            slots[slotIndex].itemNameText.text = GetItemName(slotIndex);
            slots[slotIndex].itemPriceText.text = GetCurrentItemPrice(slotIndex).ToString();
        }
    }

    public void UpdateItemBuyable()
    {
        for (int slotIndex = 0; slotIndex < slots.Count; slotIndex++)
        {
            if (GameManager.Instance.currentCurrency >= itemsToDisplay[slotIndex].value)
            {
                slots[slotIndex].background.color = ShowItemBuyable(true);
            }
            else
            {
                slots[slotIndex].background.color = ShowItemBuyable(false);
            }
        }
    }

    // Function to get the name of an item based on its index
    private string GetItemName(int index)
    {
        return itemsToDisplay[index].name;
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