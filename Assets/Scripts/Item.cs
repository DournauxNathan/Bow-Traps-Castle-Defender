using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Item : XRBaseInteractable
{
    [Header("REFS")]
    public Shop shopManager;

    [Header("PROPERTIES")]
    public int iD;
    public new string name;
    public int value;
    public bool isSold = false;

    public void Purchase()
    {
        shopManager.BuyItem(iD);
    }

    public void Sold()
    {
        isSold = true;
        gameObject.SetActive(false);
    }
}
