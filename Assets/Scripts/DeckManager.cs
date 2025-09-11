using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;
public class DeckManager : MonoBehaviour
{
	private int deckCount;
	public Transform focusOnCardPosition;
	public Transform gridlayoutPosition;
	public Transform popUpPosition;
	public List<Transform> cardLocation;
	public GameObject smallCardPrefab;
	private GameObject focusOnCard;
	public GridLayoutGroup gridLayout;
	private Dictionary<CardData, int> myCardList;
	private Dictionary<CardData, int> myDeckList;
	private List<CardData> cardDatabase;
	private List<GameObject> dummyCardObjectList;
	private List<GameObject> deckCardObjectList;



	public GameObject dummyServentCardPrefab;
	public GameObject dummySpellCardPrefab;

	public List<Sprite> cardImageList;

	private Dictionary<CardData, int> currentCardList;
	private int currentPage;
	private int pageLimit;
	public static DeckManager Inst{get; private set;}
	public TMP_Text pageNumber;
	public TMP_Text searchText;
	public TMP_Text deckCountText;
	public GameObject popUpMessage;
	private GameObject onMessage;
	public GameObject backButton;
	public GameObject nextButton;
	public ScrollRect scrollRect;

	public Dictionary<string, int> cardHashMap;
	void Awake()
	{Inst = this;}



	public void FocusOnCard(CardData cardData)
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
		}

		focusOnCard = Instantiate(selectedCardPrefab,
		new Vector3(0, 0, 0), Utils.QI);

		focusOnCard.GetComponent<Card>().SetCard(cardData, cardImageList[cardHashMap[cardData.GetCardNum()]]);
		focusOnCard.transform.SetParent(focusOnCardPosition);
		focusOnCard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		focusOnCard.transform.localPosition = new Vector3(0,0,0);

	}

	public void LoadDeck()
	{
		cardDatabase = DataController.Inst.LoadCardDatabase();

		currentPage = 0;
		myCardList = new();
		myDeckList = new();
		deckCardObjectList = new();
		dummyCardObjectList = new();
		//scrollRect.normalizedPosition = new Vector2(1, 1);

		cardHashMap = DataController.Inst.LoadCardHashMap();

		foreach (KeyValuePair<string, int> value in PlayerData.saveData.cardList)
		{ myCardList.Add(cardDatabase[cardHashMap[value.Key]], value.Value); }

		foreach (KeyValuePair<string, int> value in PlayerData.saveData.deck)
		{ myDeckList.Add(cardDatabase[cardHashMap[value.Key]], value.Value); }
		CreatePage();
	}



	public void UnFocusCard()
	{
		if(focusOnCard != null)
		Destroy(focusOnCard);
	}
	public void SaveDeck()
	{
		AlertPopUpMessage("해당 덱을 저장했습니다");
		Dictionary<string, int> dumb = new Dictionary<string, int>();
		foreach(KeyValuePair<CardData, int> value in myDeckList)
		{dumb.Add(value.Key.GetCardNum().ToString(), value.Value);}

		PlayerData.saveData.deck = dumb;

		dumb = new Dictionary<string, int>();

		foreach (KeyValuePair<CardData, int> value in myCardList)
		{ dumb.Add(value.Key.GetCardNum().ToString(), value.Value); }


		PlayerData.saveData.cardList = dumb;
		DataController.Inst.SaveData(PlayerData.saveData);
	}
	
	public void CreatePage()
	{
		currentCardList = new Dictionary<CardData, int>();

		UpdatePage();
		UpdateDeckPage();
	}


	public void AlertPopUpMessage(string value)
	{
		if(onMessage == null)
		{onMessage = Instantiate(popUpMessage, popUpPosition);}
		else
		{
			Destroy(onMessage.gameObject);
			onMessage = Instantiate(popUpMessage, popUpPosition);
		}
		onMessage.GetComponent<AlertMessage>().SetText(value);
		StartCoroutine(onMessage.GetComponent<AlertMessage>().FadeAway());
	}

	

	public void UpdatePage()
	{
		int count = 0;

		int totalPages = (myCardList.Count + 5) / 6;
		pageLimit = totalPages - 1;

		if (currentPage > pageLimit)
			currentPage = pageLimit;

		currentCardList.Clear();
		foreach (GameObject gameObject in dummyCardObjectList)
		{ Destroy(gameObject); }

		List<CardData> cardList = new List<CardData>(myCardList.Keys);

		int startIndex = currentPage * 6;
		int cardsToShow = Mathf.Min(6, myCardList.Count - startIndex);

		for (int i = 0; i < cardsToShow; ++i)
		{
			CardData cardData = cardList[startIndex + i];
			currentCardList.Add(cardData, myCardList[cardData]);
		}

		foreach (KeyValuePair<CardData, int> item in currentCardList)
		{
			bool locked = false;

			if (item.Value == 0)
			{ locked = true; }

			if (myDeckList.ContainsKey(item.Key))
			{
				if (myDeckList[item.Key] == item.Value)
				{ locked = true; }

				if (myDeckList[item.Key] == 3)
				{ locked = true; }
			}
			GameObject selectedCardPrefab = null;

			switch (item.Key.GetCardType())
			{
				case ECardType.Servent:
					selectedCardPrefab = dummyServentCardPrefab;
					break;
				case ECardType.Spell:
					selectedCardPrefab = dummySpellCardPrefab;
					break;
			}

			GameObject cardObject = Instantiate(selectedCardPrefab,
			new Vector3(0, 0, 0), Utils.QI);

			cardObject.GetComponent<Card>().Init(item.Key, count, (clickedSlot, eventData) => {
				AddCard(clickedSlot.cardData, clickedSlot.slotCount, locked);
			});
			cardObject.GetComponent<Card>().SetCard(item.Key, cardImageList[cardHashMap[item.Key.GetCardNum()]]);

			cardObject.transform.SetParent(cardLocation[count].transform);
			cardObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
			cardObject.transform.localPosition = new Vector3(0, 0, 0);

			dummyCardObjectList.Add(cardObject);
			count++;
		}

		if (currentPage == 0)
			backButton.SetActive(false);
		else
			backButton.SetActive(true);

		if (currentPage == pageLimit)
			nextButton.SetActive(false);
		else
			nextButton.SetActive(true);

		pageNumber.text = (currentPage + 1) + " / " + (pageLimit + 1);
	}

	public void ChangePage(bool value)
	{
		if(value)
		{currentPage++;}
		else{currentPage--;}

		if(currentPage < 0)
		{currentPage = 0;}

		if(currentPage > pageLimit)
		{currentPage = pageLimit;}

		UpdatePage();
	}

	public void UpdateDeckPage()
	{
		deckCount = 0;
		foreach(GameObject gameObject in deckCardObjectList)
		{Destroy(gameObject);}

		foreach(KeyValuePair<CardData, int> value in myDeckList)
		{
			GameObject smallCard = Instantiate(smallCardPrefab, new Vector3(0,0,0) , Utils.QI);
			deckCardObjectList.Add(smallCard);

			smallCard.transform.SetParent(gridLayout.transform);
			smallCard.GetComponent<DeckCard>().SetCard(value.Key, value.Value);


			smallCard.GetComponent<DeckCard>().Init(
			(deckCard, eventData) =>
			{
				DeleteCard(deckCard.GetCardData());
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

	public void AddCard(CardData value, int order, bool locked)
	{
		if(deckCount == 30)
		{
			AlertPopUpMessage("덱이 가득 차서 더 이상 카드를 추가할 수 없습니다");
			return;
		}

		if(locked)
		{
			AlertPopUpMessage("해당 카드를 더 이상 추가할 수 없습니다");
			return;
		}
		CardData battleCardData = value ;

		if (!myDeckList.ContainsKey(battleCardData))
		{
			myDeckList.Add(battleCardData, 1);
			PlayerData.saveData.deck.Add(value.GetCardNum().ToString(), 1);
		}
		else
		{
			myDeckList[battleCardData]++;
			PlayerData.saveData.deck[value.GetCardNum().ToString()]++;
		}


		GameObject selectedCardPrefab = null;

		switch (value.GetCardType())
		{
			case ECardType.Servent:
				selectedCardPrefab = dummyServentCardPrefab;
				break;
			case ECardType.Spell:
				selectedCardPrefab = dummySpellCardPrefab;
				break;

		}


		GameObject cardObject = Instantiate(selectedCardPrefab,
		cardLocation[order].position, Utils.QI);

		if(cardObject == null)
		{ Debug.LogError("Card object is null!"); return; }

		cardObject.GetComponent<Card>().SetCard(value, cardImageList[cardHashMap[value.GetCardNum()]]);

		cardObject.transform.SetParent(this.transform);
		cardObject.GetComponent<Card>().StartMoveAndScale(gridlayoutPosition.position);
		UpdatePage();
		UpdateDeckPage();
	}

	public void DeleteCard(CardData value)
	{
		myDeckList[value]--;
		PlayerData.saveData.deck[value.GetCardNum().ToString()]--;

		if (myDeckList[value] == 0)
		{
			myDeckList.Remove(value);
			PlayerData.saveData.deck.Remove(value.GetCardNum().ToString());
		}


		GameObject selectedCardPrefab = null;

		switch (value.GetCardType())
		{
			case ECardType.Servent:
				selectedCardPrefab = dummyServentCardPrefab;
				break;
			case ECardType.Spell:
				selectedCardPrefab = dummySpellCardPrefab;
				break;

		}


		GameObject cardObject = Instantiate(
			selectedCardPrefab,
		gridlayoutPosition.position, Utils.QI);

		cardObject.GetComponent<Card>().SetCard(value, cardImageList[cardHashMap[value.GetCardNum()]]);



		cardObject.transform.SetParent(transform);
		cardObject.GetComponent<Card>().StartMoveAndScale(transform.position);

		UpdatePage();
		UpdateDeckPage();
	}

}