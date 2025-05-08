using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class DungeonItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    int itemNum;
    int itemOrder;
    
    Item item;
    int count;
    public TMP_Text countText;
    public Image itemImage;

    public void OnPointerClick(PointerEventData eventData)
    {DungeonManager.Inst.SelectUsingItem(item);}
    public void OnPointerEnter(PointerEventData eventData)
    {DungeonManager.Inst.ShowItemDescription(itemNum);}
    public void OnPointerExit(PointerEventData eventData)
    {DungeonManager.Inst.HideItemDescription();}

    

    public void SetUp(Item item, int count, Sprite sprite)
    {
        this.item = item;
        itemNum = Int32.Parse(item.GetNum());
        this.count = count;
        countText.text = count.ToString();
        itemImage.sprite = sprite;
    }

    public Item GetItem()
    {return item;}
    public int GetCount()
    {return count;}
}
