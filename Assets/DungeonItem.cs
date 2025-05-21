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
    {
        if(item.GetItemCategory() == EItemCategory.ETool)
        ItemWindow.Inst.SelectUsingItem(item);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {ItemWindow.Inst.ShowItemDescription(itemNum);}
    public void OnPointerExit(PointerEventData eventData)
    {ItemWindow.Inst.HideItemDescription();}

    

    public void SetUp(Item item, int count, Sprite sprite)
    {
        this.item = item;
        itemNum = Int32.Parse(item.GetNum());
        this.count = count;
        countText.text = count.ToString();
        itemImage.sprite = sprite;
    }


	public void SetUp(Item item, Sprite sprite)
	{
		this.item = item;
		itemNum = Int32.Parse(item.GetNum());
		itemImage.sprite = sprite;
	}

	public Item GetItem()
    {return item;}
    public int GetCount()
    {return count;}
}
