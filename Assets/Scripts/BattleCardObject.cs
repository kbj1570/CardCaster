using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Collections;
using Coffee.UIEffects; 



public class BattleCardObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public UIEffect uIEffect;
	public TMP_Text nameTMP;
	public TMP_Text forceTMP;
	public TMP_Text descriptionTMP;
	public TMP_Text costTMP;
	public Sprite cardBack;
	public BattleCardData cardData;
	public GameObject cardHighlightBorder;

	public GridLayoutGroup forceAttribute;
	bool isFront;
	bool isUsable;
	int currentCost;
	public int cardOrder;
	public PRS originPRS;
	public Vector3 originPosition;
	public ECardType cardType;

	public Image fireElement;
	public Image waterElement;
	public Image earthElement;
	public Image windElement;
	public Image darknessElement;
	public Image lightElement;

	public bool locked = false;
	private Sequence currentSequence;

	void Start()
	{
		this.transform.localScale = Vector3.zero; // 처음 크기를 0으로 설정
		StartCoroutine(AppearAfterDelay(0.3f)); // 0.3초 후 애니메이션 실행
	}

	public void HideAndReveal(bool flag)
	{
		if (currentSequence != null && currentSequence.IsActive())
			currentSequence.Kill();
		
		if (flag)
		{
			// 숨기기: 더 빠르게 사라지도록 Ease.InBack 사용
			currentSequence = DOTween.Sequence()
			.Append(transform.DOMoveY(originPRS.pos.y - 330, 0.5f).SetEase(Ease.InBack));
		}
		else
		{
			// 나타나기: 목표 위치를 살짝 넘었다가 돌아오는 효과
			currentSequence = DOTween.Sequence()
			.Append(transform.DOMoveY(originPRS.pos.y + 30, 0.3f).SetEase(Ease.OutQuad)) // 살짝 위로 넘기기
			.Append(transform.DOMoveY(originPRS.pos.y, 0.2f).SetEase(Ease.OutBack)); // 부드럽게 착지
		}
	}

	

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
	public bool GetIsUsable(){return isUsable;}

	public void SetCardOrder(int value)
	{this.cardOrder = value;}

	public int GetCardOrder()
	{return cardOrder;}

	public ECardType GetCardType()
	{return cardType;}

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

	public int GetCurrentCost()
	{return currentCost;}
	public void UpdateIsUsable()
	{isUsable = (currentCost == 0);}

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

	public void SetLock(bool value)
	{this.locked = value;}

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
		if(locked)
		{return;}

		BattleManager.Inst.CardBeginDrag(this.gameObject);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if(locked)
		{return;}

		StartCoroutine(BattleManager.Inst.CardEndDrag(this, BattleManager.Inst.ReturnMouseOnField()));
	}

	public void OnDrag(PointerEventData eventData)
	{
		if(locked)
		{return;}
		this.transform.localScale = new Vector3(0.4f, 0.4f, 1);
		this.transform.position = originPRS.pos;
		BattleManager.Inst.CardOnDrag(this.gameObject);
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (locked) return;

		if (currentSequence != null && currentSequence.IsActive())
			currentSequence.Kill();
		

		currentSequence = DOTween.Sequence()
			.Append(transform.DOScale(new Vector3(0.7f, 0.7f, 1), 0.13f).SetEase(Ease.InOutQuad))
			.Append(transform.DOMoveY(originPRS.pos.y + 130, 0.13f).SetEase(Ease.OutCirc));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (locked) return;

		if (currentSequence != null && currentSequence.IsActive())
			currentSequence.Kill();

		currentSequence = DOTween.Sequence()
			.Append(transform.DOScale(new Vector3(0.4f, 0.4f, 1), 0.07f).SetEase(Ease.InOutQuad))
			.Append(transform.DOMove(originPRS.pos, 0.07f).SetEase(Ease.OutCirc));
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			BattleManager.Inst.DiscardCard(this);
		}
	}

	public void SetOriginPosition(Vector3 value)
	{originPosition = value;}
}
