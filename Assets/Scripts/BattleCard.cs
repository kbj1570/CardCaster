using Coffee.UIEffects; 
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class BattleCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public UIEffect uIEffect;
	public TMP_Text nameTMP;
	public TMP_Text descTMP;
	public TMP_Text costTMP;
	public Sprite cardBack;
	public BattleCardData cardData;
	public GameObject cardHighlightBorder;


	float duration = 0.35f; // 전체 이동 시간
	float scaleFactor = 0.7f; // 최대 커지는 배율

	public GridLayoutGroup forceAttribute;
	bool isFront;
	bool isUsable;
	int currentCost;
	public int cardOrder;
	public PRS originPRS;
	public Vector3 originPosition;
	public ECardType cardType;


	public int slotCount;

	public Image fireElement;
	public Image waterElement;
	public Image earthElement;
	public Image windElement;
	public Image darknessElement;
	public Image lightElement;

	public bool locked = false;
	public Sequence currentSequence;


	public Action<BattleCard, PointerEventData> OnClickAction;
	public Action<BattleCard, PointerEventData> OnBeginDragAction;
	public Action<BattleCard, PointerEventData> OnDragAction;
	public Action<BattleCard, PointerEventData> OnEndDragAction;
	public Action<BattleCard, PointerEventData> OnPointerEnterAction;
	public Action<BattleCard, PointerEventData> OnPointerExitAction;

	void Start()
	{
		
	}

	public void InitiateActionInBattle()
	{
		this.transform.localScale = Vector3.zero; // 처음 크기를 0으로 설정
		StartCoroutine(AppearAfterDelay(0.3f)); // 0.3초 후 애니메이션 실행
	}

	public void Init(Action<BattleCard, PointerEventData> clickAction,
					Action<BattleCard, PointerEventData> beginDragAction,
					Action<BattleCard, PointerEventData> onDragAction,
					Action<BattleCard, PointerEventData> endDragAction,
					Action<BattleCard, PointerEventData> enterAction,
					Action<BattleCard, PointerEventData> exitAction

		)
	{
		OnClickAction = clickAction;
		OnBeginDragAction = beginDragAction;
		OnDragAction = onDragAction;
		OnEndDragAction = endDragAction;
		OnPointerEnterAction = enterAction;
		OnPointerExitAction = exitAction;
	}

	public void Init(BattleCardData data, int slotCount, Action<BattleCard, PointerEventData> clickAction)
	{
		cardData = data;
		this.slotCount = slotCount;
		OnClickAction = clickAction;
	}


	public void HideAndReveal(bool flag)
	{
		if (currentSequence != null && currentSequence.IsActive())
			currentSequence.Kill();
		
		if (flag)
		{
			currentSequence = DOTween.Sequence()
			.Append(transform.DOMoveY(originPRS.pos.y - 330, 0.5f).SetEase(Ease.InBack));
		}
		else
		{
			currentSequence = DOTween.Sequence()
			.Append(transform.DOMoveY(originPRS.pos.y + 30, 0.3f).SetEase(Ease.OutQuad))
			.Append(transform.DOMoveY(originPRS.pos.y, 0.2f).SetEase(Ease.OutBack));
		}
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


	public bool GetIsUsable() { return isUsable; }

	public void SetCardOrder(int value)
	{ this.cardOrder = value; }

	public int GetCardOrder()
	{ return cardOrder; }

	public ECardType GetCardType()
	{ return cardType; }

	public int GetCurrentCost()
	{ return currentCost; }
	public void UpdateIsUsable()
	{ isUsable = (currentCost == 0); }

	public void SetCard(BattleCardData cardData)
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
			ServentCardData serventCardData = this.cardData as ServentCardData;

			Image image = null;
			switch (serventCardData.GetAttribute())
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
	{ this.locked = value; }



	IEnumerator AppearAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay); // 0.3초 기다림

		if (!locked)
		{
			locked = true;
			Sequence seq = DOTween.Sequence();
			seq.Append(transform.DOScale(new Vector3(0.4f, 0.4f, 1), 0.2f).SetEase(Ease.InOutQuad));
			seq.AppendCallback(() => locked = false);
		}
	}

	public BattleCardData GetCardData(){return cardData;}



	public void UpdateCardCost(int cost)
	{
		currentCost = this.cardData.GetCardCost() - cost;
		if(currentCost < 0){currentCost = 0;}
		costTMP.text = currentCost.ToString();

		if(uIEffect != null)
		uIEffect.enabled = currentCost == 0;

	}

	public void SendMissile(Transform alertPoint, Transform targetPoint)
	{
		locked = true;
		Sequence seq = DOTween.Sequence();

		seq.Append(transform.DOMove(alertPoint.position, 0.3f).SetEase(Ease.OutQuad))
		.Append(transform.DOScale(new Vector3(0.7f, 0.7f, 1), 0.5f).SetEase(Ease.InOutQuad));

		seq.Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));

		seq.AppendCallback(() => BattleManager.Inst.ShotMissile(alertPoint, targetPoint));

		// 3. 일정 시간 대기 후 오브젝트 삭제
		seq.AppendInterval(0.5f); // 0.5초 기다리기
		seq.AppendCallback(() => Destroy(gameObject));

		DOTween.Kill(seq);
	}


	public void Setup(SpellCardData cardData)
	{
		this.cardData = cardData;
		nameTMP.text = this.cardData.GetCardName();
		cardType = cardData.GetCardType();
		costTMP.text = this.cardData.GetCardCost().ToString();
	}

	public void Setup(BattleCardData cardData)
	{
		this.cardData = cardData;
		nameTMP.text = this.cardData.GetCardName();
		cardType = cardData.GetCardType();
		costTMP.text = this.cardData.GetCardCost().ToString();
	}

	public void Setup(ServentCardData cardData)
	{
		this.cardData = cardData;
		nameTMP.text = this.cardData.GetCardName();
		cardType = cardData.GetCardType();
		costTMP.text = this.cardData.GetCardCost().ToString();

		Image image = null;
		switch (cardData.GetAttribute())
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


	public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
	{
		if(useDotween)
		{
			transform.DOMove(prs.pos, dotweenTime);
			transform.DORotateQuaternion(prs.rot, 1);
			transform.DOScale(prs.scale, 0.5f);
		}
		else
		{
			transform.position = prs.pos;
			transform.rotation = prs.rot;
			transform.localScale = prs.scale;
		}
	}

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
	public void OnPointerEnter(PointerEventData eventData)
	{
		OnPointerEnterAction?.Invoke(this, eventData);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		OnPointerExitAction?.Invoke(this, eventData);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClickAction?.Invoke(this, eventData);
	}

	public void SetOriginPosition(Vector3 value)
	{originPosition = value;}
}
