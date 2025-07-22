using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;
public class DeckManager : MonoBehaviour
{
	private int deckCount;
	public Transform focusOnCardPosition;
	public Transform gridlayoutPosition;
	public Transform popUpPosition;
	public List<Transform> cardLocation;
	public GameObject window;
	public GameObject cardPrefab;
	public GameObject smallCardPrefab;
	public GameObject cardFrame;
	private GameObject focusOnCard;
	public GridLayoutGroup gridLayout;
	private Dictionary<BattleCardData, int> myCardList;
	private Dictionary<BattleCardData, int> myDeckList;
	private List<CardData> cardDatabase;
	private List<BattleCardData> currentPageCardList;
	public List<GameObject> dummyCardObjectList;
	private List<GameObject> deckCardObjectList;



	public GameObject dummyServentCardPrefab;
	public GameObject dummySpellCardPrefab;
	public GameObject dummyAdventureCardPrefab;


	private Dictionary<BattleCardData, int> currentCardList;
	private int currentPage;
	private int pageLimit;

	private SaveData saveData;
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

	void Start()
	{
		saveData = DataController.Inst.LoadData();
		cardDatabase = DataController.Inst.LoadCardDatabase();

		currentPage = 0;
		myCardList = new();
		myDeckList = new();
		deckCardObjectList = new();
		//scrollRect.normalizedPosition = new Vector2(1, 1);

		cardHashMap = DataController.Inst.LoadCardHashMap();

		foreach (KeyValuePair<string, int> value in saveData.cardList)
		{ myCardList.Add(cardDatabase[cardHashMap[value.Key]] as BattleCardData, value.Value); }

		foreach (KeyValuePair<string, int> value in saveData.deck)
		{ myDeckList.Add(cardDatabase[cardHashMap[value.Key]] as BattleCardData, value.Value); }
		CreatePage();
	}

	void Awake()
	{
		Inst = this;

		
	}



	public void FocusOnCard(BattleCardData cardData)
	{
		//focusOnCard = Instantiate(dummyCardPrefabList[cardHashMap[cardData.GetCardNum()]],
		//	new Vector3(0,0,0) , Utils.QI);

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
			focusOnCard.transform.localPosition = new Vector3(0,0,0);

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
		foreach(KeyValuePair<BattleCardData, int> value in myDeckList)
		{dumb.Add(value.Key.GetCardNum().ToString(), value.Value);}

		PlayerData.saveData.deck = dumb;

		dumb = new Dictionary<string, int>();

		foreach (KeyValuePair<BattleCardData, int> value in myCardList)
		{ dumb.Add(value.Key.GetCardNum().ToString(), value.Value); }


		PlayerData.saveData.cardList = dumb;
		DataController.Inst.SaveData(PlayerData.saveData);
	}
	public void LoadDeck()
	{
	   Dictionary<CardData, int> dumb = new Dictionary<CardData, int>();
	}
	public void SaveCardList()
	{
		
	}
	public void LoadCardList()
	{
		
	}
	public void CreatePage()
	{
		currentCardList = new Dictionary<BattleCardData, int>();

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
		onMessage.GetComponent<PopUpMessage>().SetText(value);
	}

	

	public void UpdatePage()
	{
		int count = 0;

		pageLimit = myCardList.Count / 6 + 1;
		int remainder = myCardList.Count % 6;

		if(remainder == 0)
		{ pageLimit--; }

		currentCardList.Clear();
		foreach(GameObject gameObject in dummyCardObjectList)
		{Destroy(gameObject);}

		List<BattleCardData> cardList = new List<BattleCardData>(myCardList.Keys);

		if(currentPage != pageLimit)
		{remainder = 6;}

		for(int i = 0; i < remainder; ++i)
		{
			currentCardList.Add(cardList[(currentPage * 6) + i], myCardList[cardList[(currentPage * 6) + i]]);
		}

		foreach (KeyValuePair<BattleCardData, int> item in currentCardList)
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
				case ECardType.Adventure:
					selectedCardPrefab = dummyAdventureCardPrefab;
					break;

			}


			GameObject cardObject = Instantiate(selectedCardPrefab,
			new Vector3(0, 0, 0), Utils.QI);

			cardObject.GetComponent<Card>().Init(item.Key, count, (clickedSlot, eventData) => {
				AddCard(clickedSlot.cardData, clickedSlot.slotCount, locked);
			});
			cardObject.GetComponent<Card>().SetCard(item.Key);


			cardObject.transform.SetParent(cardLocation[count].transform);
			cardObject.transform.localScale = new Vector3(0.7f,0.7f, 1f);
			cardObject.transform.localPosition = new Vector3(0,0,0);
			
			dummyCardObjectList.Add(cardObject);
			count++;
		}
		if(currentPage == 0)
		backButton.SetActive(false);
		else
		backButton.SetActive(true);

		if(currentPage == pageLimit - 1)
		nextButton.SetActive(false);
		else
		nextButton.SetActive(true);

		pageNumber.text = (currentPage + 1) + " / " + (pageLimit);        
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

		foreach(KeyValuePair<BattleCardData, int> value in myDeckList)
		{
			GameObject gameObject = Instantiate(smallCardPrefab, new Vector3(0,0,0) , Utils.QI);
			deckCardObjectList.Add(gameObject);

			gameObject.transform.SetParent(gridLayout.transform);
			gameObject.GetComponent<DeckCard>().SetCard(value.Key, value.Value);

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
		BattleCardData battleCardData = value as BattleCardData;

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
			case ECardType.Adventure:
				selectedCardPrefab = dummyAdventureCardPrefab;
				break;

		}


		GameObject cardObject = Instantiate(selectedCardPrefab,
		cardLocation[order].position, Utils.QI);

		cardObject.GetComponent<DummyCard>().SetCard(value);

		cardObject.transform.SetParent(window.transform);
		cardObject.GetComponent<DummyCard>().StartMoveAndScale(gridlayoutPosition.position);
		UpdatePage();
		UpdateDeckPage();
	}

	public void DeleteCard(BattleCardData value)
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
			case ECardType.Adventure:
				selectedCardPrefab = dummyAdventureCardPrefab;
				break;

		}


		GameObject cardObject = Instantiate(selectedCardPrefab,
		gridlayoutPosition.position, Utils.QI);

		cardObject.GetComponent<DummyCard>().SetCard(value);



		cardObject.transform.SetParent(window.transform);
		cardObject.GetComponent<DummyCard>().StartMoveAndScale(window.transform.position);

		UpdatePage();
		UpdateDeckPage();
	}

}