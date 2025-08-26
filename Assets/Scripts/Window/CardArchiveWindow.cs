
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CardArchiveWindow : Window
{

	ECardType currentCardType;

	public TMP_Text pageNumber;
	public TMP_Text cardCountText;
	private int currentPage;
	private int pageLimit;

	public GameObject backButton;
	public GameObject nextButton;

	private Dictionary<CardData, bool> currentCardList;

	private Dictionary<CardData, bool> cardArchiveList;

	private Dictionary<CardData, bool> serventCardList;
	private Dictionary<CardData, bool> spellCardList;
	private Dictionary<CardData, bool> selectedCardList;

	public GameObject dummyServentCardPrefab;
	public GameObject dummySpellCardPrefab;
	public GameObject dummyCardPrefab;

	public List<Transform> cardLocation;
	List<GameObject> cardObjectList;

	Dictionary<string, int> cardHashMap;

	private List<CardData> cardDatabase;

	public GameObject cardStoryDescWindow;

	public List<Sprite> cardImageList;


	void Start()
	{
		ScaleZero();
		cardDatabase = DataController.Inst.LoadCardDatabase();
		cardHashMap = DataController.Inst.LoadCardHashMap();

		cardArchiveList= new Dictionary<CardData, bool>();

		serventCardList = new Dictionary<CardData, bool>();
		spellCardList = new Dictionary<CardData, bool>();

		currentCardList = new Dictionary<CardData, bool>();
		cardObjectList = new List<GameObject>();

		foreach (KeyValuePair<string, bool> value in PlayerData.saveData.cardArchiveList)
		{cardArchiveList.Add(cardDatabase[cardHashMap[value.Key]], value.Value);}

		foreach (KeyValuePair<CardData, bool> value in cardArchiveList)
		{
			switch (value.Key.GetCardType())
			{
				case ECardType.Servent:
					serventCardList.Add(value.Key, value.Value);
					break;
				case ECardType.Spell:
					spellCardList.Add(value.Key, value.Value);
					break;
			}
		}

		currentPage = 0;
		UpdatePageNum();
		UpdatePage();
	}
	public void SetCardType(int cardType)
	{
		currentCardType = (ECardType)cardType;
		currentPage = 0;
		UpdatePageNum();

		ClearCards();
		UpdatePage();
	}

	public void UpdatePage()
	{

		switch (currentCardType)
		{
			case ECardType.Servent:
				selectedCardList = serventCardList;
				break;

			case ECardType.Spell:	
				selectedCardList = spellCardList;
				break;

			case ECardType.None:
				selectedCardList = cardArchiveList;
				break;
		}

		int count = 0;
		int cardCount = 0;

		pageLimit = selectedCardList.Count / 6;
		int remainder = selectedCardList.Count % 6;

		if (remainder != 0)
		{ pageLimit++; }

		currentCardList.Clear();
		foreach (GameObject gameObject in cardObjectList)
		{ Destroy(gameObject); }


		if (currentPage != pageLimit - 1)
		{ 
			remainder = 6;
		}

		for (int i = 0; i < remainder; ++i)
		{
			currentCardList.Add(selectedCardList.Keys.ToList()[(currentPage * 6) + i], selectedCardList.Values.ToList()[(currentPage * 6) + i]);
		}

		foreach (KeyValuePair<CardData, bool> valuePair in selectedCardList)
		{
			if(valuePair.Value)
			{cardCount++;}
		}

		foreach (KeyValuePair<CardData, bool> valuePair in currentCardList)
		{
			bool locked = valuePair.Value;

			GameObject selectedCardPrefab = null;

			

			if(valuePair.Value)
			{
				switch (valuePair.Key.GetCardType())
				{
					case ECardType.Servent:
						selectedCardPrefab = dummyServentCardPrefab;
						break;
					case ECardType.Spell:
						selectedCardPrefab = dummySpellCardPrefab;
						break;
				}
			}
			else
			{
				selectedCardPrefab = dummyCardPrefab;
			}


			GameObject cardObject = Instantiate(selectedCardPrefab, new Vector3(0, 0, 0), Utils.QI);

			if (valuePair.Value)
			{
				cardObject.GetComponent<Card>().Init(valuePair.Key as BattleCardData, count, (clickedSlot, eventData) =>
				{
					OpenCardStoryDesc(clickedSlot.cardData);
				});
				cardObject.GetComponent<Card>().SetCard(valuePair.Key, cardImageList[cardHashMap[valuePair.Key.GetCardNum()]]);
			}
			
			cardObject.transform.SetParent(cardLocation[count].transform);
			cardObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
			cardObject.transform.localPosition = new Vector3(0, 0, 0);

			cardObjectList.Add(cardObject);
			count++;
		}
		if (currentPage == 0)
			backButton.SetActive(false);
		else
			backButton.SetActive(true);

		if (currentPage == pageLimit - 1)
			nextButton.SetActive(false);
		else
			nextButton.SetActive(true);

		pageNumber.text = (currentPage + 1) + " / " + (pageLimit);
		cardCountText.text = cardCount + " / " + selectedCardList.Count;
	}

	private void OpenCardStoryDesc(CardData cardData)
	{
		cardStoryDescWindow.GetComponent<CardStoryDescWindow>().SetCardData(cardData);
		cardStoryDescWindow.GetComponent<Window>().OnOff();
	}

	private void ClearCards()
	{

		currentCardList.Clear();
		foreach (GameObject child in cardObjectList)
		{
			Destroy(child.gameObject);
		}
	}

	public void ChangePage(bool value)
	{
		if (value)
		{ currentPage++; }
		else { currentPage--; }

		if (currentPage < 0)
		{ currentPage = 0; }

		if (currentPage > pageLimit)
		{ currentPage = pageLimit; }

		UpdatePage();
	}

	public void UpdatePageNum()
	{pageNumber.text = (currentPage + 1) + " / " + (pageLimit + 1);}

	public void SetCurrentPage(int value)
	{currentPage = value;}

	public int GetCurrentPage()
	{return currentPage;}
}