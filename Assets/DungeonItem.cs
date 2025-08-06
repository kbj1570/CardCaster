using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class DungeonItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    int itemNum;
    int itemOrder;
    
    Item item;
    int count;
    public TMP_Text countText;
    public Image itemImage;

	public Action<DungeonItem, PointerEventData> OnClickAction;
	public Action<DungeonItem, PointerEventData> OnPointerEnterAction;
	public Action<DungeonItem, PointerEventData> OnPointerExitAction;
	public Action<DungeonItem, PointerEventData> OnBeginDragAction;
	public Action<DungeonItem, PointerEventData> OnDragAction;
	public Action<DungeonItem, PointerEventData> OnEndDragAction;

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


	public void Init(Action<DungeonItem, PointerEventData> clickAction,
					Action<DungeonItem, PointerEventData> enterAction,
					Action<DungeonItem, PointerEventData> exitAction,
					Action<DungeonItem, PointerEventData> beginDragAction,
					Action<DungeonItem, PointerEventData> onDragAction,
					Action<DungeonItem, PointerEventData> endDragAntion
		)
	{
		OnClickAction = clickAction;
		OnPointerEnterAction = enterAction;
		OnPointerExitAction = exitAction;
		OnBeginDragAction = beginDragAction;
		OnDragAction = onDragAction;
		OnEndDragAction = endDragAntion;
	}


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
