using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

public class InventoryItem : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text itemCountText;
    public ItemSO itemData;
    private int itemCount;

    private MerchantMannager merchantMannager;

    public void SetItemData(ItemSO value)
    {
        itemData = value;
        itemNameText.text = itemData.GetItemName();
    }


    public void SetItemCount(int value)
    {
        itemCount = value;
        itemCountText.text = itemCount.ToString();
    }

    public ItemSO GetItemData()
    {return itemData;}

    public void SetMannager(MerchantMannager value)
    {
        merchantMannager = value;
    }

    public void SelectThis()
    {
        merchantMannager.SelectItem(itemData, itemCount);
    }

    public void SellThis()
    {
        merchantMannager.SetSellMenu();
        merchantMannager.SelectItem(itemData, itemCount);
    }

    public void PurchaseThis()
    {
        merchantMannager.SetPurchaseMenu();
        merchantMannager.SelectItem(itemData, itemCount);
    }
}
