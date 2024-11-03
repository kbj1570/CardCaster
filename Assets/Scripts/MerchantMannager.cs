using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MerchantMannager : MonoBehaviour
{
    public int itemLimit = 6;
    public List<ItemSO> itemList;
    public TMP_Text goldText;
    public Dictionary<ItemSO, int> merchantItemDictionary;
    public Dictionary<ItemSO, int> inventoryItemDictionary;
    public GridLayoutGroup merchantGridLayOut;
    public GridLayoutGroup inventoryGridLayOut;
    public GameObject merchantItemPrefab;
    public GameObject inventoryItemPreFab;
    public GameObject countWindow;
    public GameObject merchantWindow;


    List<GameObject> merchantItemObjectList;
    List<GameObject> inventoryItemObjectList;

    public ItemSO selectedItem;

    public EMerchantMenu currentMenu;

    void Start()
    {
        merchantItemDictionary = new Dictionary<ItemSO, int>();
        inventoryItemDictionary = new Dictionary<ItemSO, int>();
        merchantItemObjectList = new List<GameObject>();
        inventoryItemObjectList = new List<GameObject>();

        merchantWindow.GetComponent<Window>().OnOff();
    }

    public void Show()
    {
        CreateItemDictionary();


        ShowMerchantItem();
        ShowPlayerItem();
    }


    public void CreateItemDictionary()
    {
        for(int i = 0; i < itemLimit; ++i)
        {
            int random = Random.Range(0, itemList.Count);

            if(!merchantItemDictionary.ContainsKey(itemList[random]))
            {merchantItemDictionary.Add(itemList[random], 1);}
            else
            {merchantItemDictionary[itemList[random]]++;}
        }

        inventoryItemDictionary = PlayerManager.Inst.GetSellItem();
    }

    public void ShowMerchantItem()
    {
        foreach(GameObject value in merchantItemObjectList)
        {
            Destroy(value);
        }

        goldText.text= PlayerManager.Inst.GetGold().ToString();

        foreach(KeyValuePair<ItemSO, int> value in merchantItemDictionary)
        {
            GameObject gameObject = Instantiate(merchantItemPrefab, new Vector3() , Utils.QI);
            gameObject.transform.SetParent(merchantGridLayOut.transform);
            gameObject.GetComponent<InventoryItem>().SetItemData(value.Key);
            gameObject.GetComponent<InventoryItem>().SetItemCount(value.Value);
            gameObject.GetComponent<InventoryItem>().SetMannager(this);

            merchantItemObjectList.Add(gameObject);
        }
    }

    public void ShowPlayerItem()
    {
        foreach(GameObject value in inventoryItemObjectList)
        {
            Destroy(value);
        }

        goldText.text= PlayerManager.Inst.GetGold().ToString();

        foreach(KeyValuePair<ItemSO, int> value in PlayerManager.Inst.GetSellItem())
        {
            GameObject gameObject = Instantiate(inventoryItemPreFab, new Vector3() , Utils.QI);
            gameObject.transform.SetParent(inventoryGridLayOut.transform);
            gameObject.GetComponent<InventoryItem>().SetItemData(value.Key);
            gameObject.GetComponent<InventoryItem>().SetItemCount(value.Value);
            gameObject.GetComponent<InventoryItem>().SetMannager(this);

            merchantItemObjectList.Add(gameObject);
        }

    }

    public void PurchaseItem()
    {
        if(currentMenu.Equals(EMerchantMenu.EPurchase))
        {
            int c = merchantItemDictionary[selectedItem] - countWindow.GetComponent<CountWindow>().GetCount();

            if(c == 0)
            {merchantItemDictionary.Remove(selectedItem);}
            else
            {merchantItemDictionary[selectedItem] = c;}

            PlayerManager.Inst.AddItem(selectedItem, countWindow.GetComponent<CountWindow>().GetCount());
            PlayerManager.Inst.LoseGold(countWindow.GetComponent<CountWindow>().GetCount() * selectedItem.GetPrice());

            
        }
        else{SellItem();}

        countWindow.GetComponent<CountWindow>().OnOff();

        ShowMerchantItem();
        ShowPlayerItem();
        
    }

    public void SellItem()
    {
        PlayerManager.Inst.DeleteItem(selectedItem, countWindow.GetComponent<CountWindow>().GetCount());
        PlayerManager.Inst.GainGold(countWindow.GetComponent<CountWindow>().GetCount() * selectedItem.GetPrice());
    }

    public void SelectItem(ItemSO value, int itemCount)
    {
        this.selectedItem = value;

        countWindow.GetComponent<CountWindow>().SetCount(1);
        countWindow.GetComponent<CountWindow>().UpdateCountText();
        countWindow.GetComponent<CountWindow>().SetLimit(itemCount);
        countWindow.GetComponent<CountWindow>().OnOff();
        
    }

    public void ExitShop()
    {SceneManager.LoadScene("SafeZone");}

    public void SetPurchaseMenu()
    {this.currentMenu = EMerchantMenu.EPurchase;}

    public void SetSellMenu()
    {this.currentMenu = EMerchantMenu.ESell;}

    public void ClearSelect()
    {selectedItem = null;}

    public enum EMerchantMenu
    {EPurchase, ESell}
}
