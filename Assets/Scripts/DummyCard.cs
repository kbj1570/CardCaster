using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;



public class DummyCard : MonoBehaviour
{
	public TMP_Text nameTMP;
	public TMP_Text forceTMP;
	public TMP_Text descriptionTMP;
	public TMP_Text costTMP;
	public Sprite cardBack;
	public CardData cardData;
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

	float duration = 0.35f; // 전체 이동 시간
	float scaleFactor = 0.7f; // 최대 커지는 배율

	public void StartMoveAndScale(Vector3 targetPosition)
	{
		Vector3 startPosition = transform.position;
		float growTime = duration * 0.2f;
		float shrinkTime = duration * 0.8f;

		Sequence sequence = DOTween.Sequence();

		// 1. 처음 20% 동안 크기가 커짐
		sequence.Append(transform.DOScale(scaleFactor, growTime));

		// 2. 크기 작아지면서 목표 위치로 이동
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

	public void Setup(CardData cardData)
	{
		this.cardData = cardData;
		nameTMP.text = this.cardData.GetCardName();
		cardType = cardData.GetCardType();
		costTMP.text = (this.cardData as ServentCardData).GetCardCost().ToString();

		if(cardData.GetCardType() == ECardType.Servent)
		{
			ServentCardData serventCardData = this.cardData as ServentCardData;

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
			

			for (int i = 0; i < serventCardData.GetForce(); ++i)
			{
				Image gameObject = Instantiate(image, forceAttribute.transform.position, Utils.QI);

				gameObject.transform.SetParent(forceAttribute.transform);

				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		// currentCost = this.cardData.GetCardCost();
		// UpdateIsUsable();
	}

	public void SetLock(bool value)
	{this.locked = value;}

	public void SetOriginPosition(Vector3 value)
	{originPosition = value;}
}
