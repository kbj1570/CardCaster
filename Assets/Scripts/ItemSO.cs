using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemSO", menuName ="Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{

    public EItemClass itemClass;

    public int itemNum;

    public string itemName;
    public string itemDescription;
    public Sprite image;
    public int price;

    public int GetItemNum(){return itemNum;}
    public string GetItemName(){return itemName;}
    public EItemClass GetItemClass(){return itemClass;}
    public int GetPrice(){return price;}
}

public enum EItemClass
{Important, Usable, Unusable}
