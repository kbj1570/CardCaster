using DG.Tweening;
using System.Collections.Generic;

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckScrollView : MonoBehaviour
{

	private int deckCount;
	public Transform focusOnCardPosition;
	public Transform gridlayoutPosition;
	public GameObject smallCardPrefab;
	private GameObject focusOnCard;
	public GridLayoutGroup gridLayout;
	private Dictionary<BattleCardData, int> myDeckList;
	private List<CardData> cardDatabase;
	private List<GameObject> deckCardObjectList;

	public Transform movePosition;
	public Transform originPosition;



	public GameObject dummyServentCardPrefab;
	public GameObject dummySpellCardPrefab;
	public GameObject dummyAdventureCardPrefab;


	private SaveData saveData;
	public TMP_Text deckCountText;
	public GameObject popUpMessage;
	public ScrollRect scrollRect;

	public Dictionary<string, int> cardHashMap;

	void Start()
	{
		//saveData = DataController.Inst.LoadData();
		//cardDatabase = DataController.Inst.LoadCardDatabase();

		//myDeckList = new();
		////scrollRect.normalizedPosition = new Vector2(1, 1);

		//cardHashMap = DataController.Inst.LoadCardHashMap();
		//deckCardObjectList = new List<GameObject>();

		//foreach (KeyValuePair<string, int> value in saveData.deck)
		//{ myDeckList.Add(cardDatabase[cardHashMap[value.Key]] as BattleCardData, value.Value); }
		//UpdateDeckScroll();
	}



	public void FocusOnCard(BattleCardData cardData)
	{
		GameObject selectedCardPrefab = null;

		switch (cardData.GetCardType())
		{
			case ECardType.Servent:
				selectedCardPrefab = dummyServentCardPrefab;
				break;
			case ECardType.Spell:
				selectedCardPrefab = dummySpellCardPrefab;
				break;
			case ECardType.Adventure:
				selectedCardPrefab = dummyAdventureCardPrefab;
				break;
		}

		focusOnCard = Instantiate(selectedCardPrefab,
		new Vector3(0, 0, 0), Utils.QI);

		focusOnCard.GetComponent<Card>().SetCard(cardData);
		focusOnCard.transform.SetParent(focusOnCardPosition);
		focusOnCard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		focusOnCard.transform.localPosition = new Vector3(0, 0, 0);

	}

	public void OpenScrollView()
	{
		StartCoroutine(IEOpenScrollView());
	}

	public void CloseScrollView()
	{
		transform.DOMove(originPosition.position, 0.2f).SetEase(Ease.InQuad);
	}

	private IEnumerator IEOpenScrollView()
	{
		transform.DOMove(movePosition.position,
		0.2f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(0.2f);
		transform.DOMove(movePosition.position + new Vector3(50, 0, 0),
		0.1f).SetEase(Ease.OutExpo);
		yield return new WaitForSeconds(0.1f);
	}

	private void IECloseScrollView()
	{
		
	}



	public void UnFocusCard()
	{
		if (focusOnCard != null)
			Destroy(focusOnCard);
	}

	public void UpdateDeckScroll()
	{
		deckCount = 0;

		foreach (GameObject gameObject in deckCardObjectList)
		{ Destroy(gameObject); }

		foreach (KeyValuePair<BattleCardData, int> value in myDeckList)
		{
			GameObject smallCard = Instantiate(smallCardPrefab, new Vector3(0, 0, 0), Utils.QI);
			deckCardObjectList.Add(smallCard);

			smallCard.transform.SetParent(gridLayout.transform);
			smallCard.GetComponent<DeckCard>().SetCard(value.Key, value.Value);

			smallCard.GetComponent<DeckCard>().Init(
			(deckCard, eventData) =>
			{
				UnFocusCard();
			}
			, // 클릭 시
			(deckCard, eventData) =>
			{
				FocusOnCard(deckCard.GetCardData());
			} // 마우스 입장
			,
			(deckCard, eventData) =>
			{
				UnFocusCard();
			} // 마우스 퇴장
			,
			null, // 드래그 시작
			null, // 드래그 중
			null// 드래그 끝
			);

			deckCount += value.Value;
		}

		deckCountText.text = deckCount.ToString() + "  /  30";
	}
}
