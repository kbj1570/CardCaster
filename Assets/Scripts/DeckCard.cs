using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
public class DeckCard : MonoBehaviour, IPointerClickHandler , IPointerEnterHandler,  IPointerExitHandler
{
	private BattleCardData cardData;
	private string cardName;
	private int count;
	private int cardCost;
	public TMP_Text cardNameText;
	public TMP_Text cardCountText;
	public TMP_Text cardCostText;
	public Image image;
	public Color purpleColor;

		public float duration = 2f; // 전체 이동 시간
	public float scaleFactor = 2f; // 최대 커지는 배율

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

	public void OnPointerClick(PointerEventData eventData)
	{
		DeckManager.Inst.DeleteCard(cardData);
		DeckManager.Inst.UnFocusCard();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{DeckManager.Inst.FocusOnCard(cardData);}

	public void OnPointerExit(PointerEventData eventData)
	{DeckManager.Inst.UnFocusCard();}
}
