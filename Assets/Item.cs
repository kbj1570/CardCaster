using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class Item : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    int itemNum;
    int itemOrder;
    
    ItemData itemData;
    int count;
    public TMP_Text countText;
    public Image itemImage;

	public Action<Item, PointerEventData> OnClickAction;
	public Action<Item, PointerEventData> OnPointerEnterAction;
	public Action<Item, PointerEventData> OnPointerExitAction;
	public Action<Item, PointerEventData> OnBeginDragAction;
	public Action<Item, PointerEventData> OnDragAction;
	public Action<Item, PointerEventData> OnEndDragAction;

	public void OnPointerClick(PointerEventData eventData)
    {OnClickAction?.Invoke(this, eventData);}
    public void OnPointerEnter(PointerEventData eventData)
    { OnPointerEnterAction?.Invoke(this, eventData); }
    public void OnPointerExit(PointerEventData eventData)
    { OnPointerExitAction?.Invoke(this, eventData); }


	public void OnBeginDrag(PointerEventData eventData)
	{OnBeginDragAction?.Invoke(this, eventData);}

	public void OnEndDrag(PointerEventData eventData)
	{OnEndDragAction?.Invoke(this, eventData);}

	public void OnDrag(PointerEventData eventData)
	{OnDragAction?.Invoke(this, eventData);}


	public void Init(Action<Item, PointerEventData> clickAction,
					Action<Item, PointerEventData> enterAction,
					Action<Item, PointerEventData> exitAction,
					Action<Item, PointerEventData> beginDragAction,
					Action<Item, PointerEventData> onDragAction,
					Action<Item, PointerEventData> endDragAntion
		)
	{
		OnClickAction = clickAction;
		OnPointerEnterAction = enterAction;
		OnPointerExitAction = exitAction;
		OnBeginDragAction = beginDragAction;
		OnDragAction = onDragAction;
		OnEndDragAction = endDragAntion;
	}


	public void SetUp(ItemData item, int count, Sprite sprite)
    {
        this.itemData = item;
        itemNum = Int32.Parse(item.GetNum());
        this.count = count;
        countText.text = count.ToString();
        itemImage.sprite = sprite;
    }


	public void SetUp(ItemData item, Sprite sprite)
	{
		this.itemData = item;
		itemNum = Int32.Parse(item.GetNum());
		itemImage.sprite = sprite;
	}

	public ItemData GetItem()
    {return itemData;}
    public int GetCount()
    {return count;}
}
