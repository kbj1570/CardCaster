using DG.Tweening;
using NUnit.Framework.Interfaces;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;



public class DummyCard : MonoBehaviour, IPointerClickHandler
{
	public TMP_Text nameTMP;
	public TMP_Text forceTMP;
	public TMP_Text descTMP;
	public TMP_Text costTMP;
	public CardData cardData;

	public GridLayoutGroup forceAttribute;
	bool isUsable;
	int currentCost;
	int cardOrder;
	public ECardType cardType;

	public Image fireElement;
	public Image waterElement;
	public Image earthElement;
	public Image windElement;
	public Image darknessElement;
	public Image lightElement;

	bool locked = false;

	float duration = 0.35f; // 전체 이동 시간
	float scaleFactor = 0.7f; // 최대 커지는 배율


	public Action<DummyCard> OnClickAction;

	public int slotCount;

	public void Init(CardData data, int slotCount, Action<DummyCard> clickAction)
	{
		cardData = data;
		this.slotCount = slotCount;
		OnClickAction = clickAction;
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		OnClickAction?.Invoke(this);
	}

	public void StartMoveAndScale(Vector3 targetPosition)
	{
		Vector3 startPosition = transform.position;
		float growTime = duration * 0.2f;
		float shrinkTime = duration * 0.8f;

		Sequence sequence = DOTween.Sequence();

		sequence.Append(transform.DOScale(scaleFactor, growTime));

		sequence.Append(transform.DOScale(0, shrinkTime).SetEase(Ease.InQuad));
		sequence.Join(transform.DOMove(targetPosition, shrinkTime).SetEase(Ease.InOutQuad));
		sequence.AppendCallback(() => Destroy(gameObject));
	}


	public CardData GetCardData(){return cardData;}
	public bool GetIsUsable(){return isUsable;}

	public void SetCardOrder(int value)
	{this.cardOrder = value;}

	public int GetCardOrder()
	{return cardOrder;}

	public ECardType GetCardType()
	{return cardType;}

	public int GetCurrentCost()
	{return currentCost;}
	public void UpdateIsUsable()
	{isUsable = (currentCost == 0);}

	public void SetCard(CardData cardData)
	{
		this.cardData = cardData;
		nameTMP.text = this.cardData.GetCardName();
		cardType = cardData.GetCardType();
		costTMP.text = this.cardData.GetCardCost().ToString();
		descTMP.text = this.cardData.GetCardDesc();

		int fontSize = 0;


		switch (this.cardData.GetCardDesc().Split(new string[] { "\r\n" }, StringSplitOptions.None).Length)
		{
			case 1:
				fontSize = 33;
				break;

			case 2:
				fontSize = 29;
				break;

			case 3:
				fontSize = 25;
				break;
		}

		descTMP.fontSize = fontSize;

		if (cardData.GetCardType() == ECardType.Servent)
		{
			ServantCardData serventCardData = this.cardData as ServantCardData;

			forceTMP.text = serventCardData.GetForce().ToString();
			Image image = null;
			switch(serventCardData.GetAttribute())
			{
				case EServentAttribute.Fire:
				image = fireElement;
				break;

				case EServentAttribute.Water:
				image = waterElement;
				break;

				case EServentAttribute.Earth:
				image = earthElement;
				break;

				case EServentAttribute.Dark:
				image = darknessElement;
				break;

				case EServentAttribute.Wind:
				image = windElement;
				break;

				case EServentAttribute.Light:
				image = lightElement;
				break;

			}
		}
	}

	public void SetLock(bool value)
	{this.locked = value;}
}
