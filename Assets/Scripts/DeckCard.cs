using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DeckCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private BattleCardData cardData;
	private Item itemData;
	private string cardName;
	private int count;
	private int cardCost;
	public TMP_Text cardNameText;
	public TMP_Text cardCountText;
	public TMP_Text cardCostText;

	public Image image;
	public Color purpleColor;
	public float duration = 2f;
	public float scaleFactor = 2f;


	public Action<DeckCard, PointerEventData> OnClickAction;
	public Action<DeckCard, PointerEventData> OnPointerEnterAction;
	public Action<DeckCard, PointerEventData> OnPointerExitAction;

	public Action<DeckCard, PointerEventData> OnBeginDragAction;
	public Action<DeckCard, PointerEventData> OnDragAction;
	public Action<DeckCard, PointerEventData> OnEndDragAction;

	public void StartMoveAndScale(Vector3 targetPosition)
	{
		Vector3 startPosition = transform.position;
		float growTime = duration * 0.2f;
		float shrinkTime = duration * 0.8f;

		Sequence sequence = DOTween.Sequence();
		sequence.Append(transform.DOScale(scaleFactor, growTime));
		sequence.Append(transform.DOScale(0, shrinkTime).SetEase(Ease.InQuad));
		sequence.Join(transform.DOMove(targetPosition, shrinkTime).SetEase(Ease.InOutQuad));
	}

	public void Init(Action<DeckCard, PointerEventData> clickAction,
					Action<DeckCard, PointerEventData> enterAction,
					Action<DeckCard, PointerEventData> exitAction,
					Action<DeckCard, PointerEventData> beginDragAction,
					Action<DeckCard, PointerEventData> onDragAction,
					Action<DeckCard, PointerEventData> endDragAntion
		)
	{
		OnClickAction = clickAction;
		OnPointerEnterAction = enterAction;
		OnPointerExitAction = exitAction;
		OnBeginDragAction = beginDragAction;
		OnDragAction = onDragAction;
		OnEndDragAction = endDragAntion;
	}

	public void SetItem(Item item, int count)
	{
		itemData = item;
		this.count = count;
		cardNameText.text = item.GetName();
		cardCountText.text = count.ToString();
	}

	public Item GetItem()
	{ return itemData; }

	public void SetItem(Item item)
	{
		itemData = item;
		cardNameText.text = item.GetName();
		cardCountText.text = "";
	}

	public void SetCard(BattleCardData value, int count)
	{
		cardData = value;
		this.count = count;
		cardNameText.text = value.GetCardName();
		cardCountText.text = count.ToString();

		if(cardData.GetCardType() == ECardType.Spell)
		image.color = purpleColor;
		if(value.GetCardType() == ECardType.Servent || value.GetCardType() == ECardType.Spell)
		{
			this.cardCost = (value as BattleCardData).GetCardCost();
			cardCostText.text = (value as BattleCardData).GetCardCost().ToString();
		}
		
	}

	public BattleCardData GetCardData()
	{
		return cardData;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClickAction?.Invoke(this, eventData);
	}
	public void OnPointerEnter(PointerEventData eventData)
	{ OnPointerEnterAction?.Invoke(this, eventData); }
	public void OnPointerExit(PointerEventData eventData)
	{ OnPointerExitAction?.Invoke(this, eventData); }


	public void OnBeginDrag(PointerEventData eventData)
	{
		OnBeginDragAction?.Invoke(this, eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		OnEndDragAction?.Invoke(this, eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		OnDragAction?.Invoke(this, eventData);
	}

}
