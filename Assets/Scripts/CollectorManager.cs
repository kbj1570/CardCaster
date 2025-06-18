using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class CollectorManager : MonoBehaviour
{
	public List<Transform> cardLocations;

	public List<IndexedCard> IndexedCards;
	List<CardData> cardDatabase;
	Dictionary<string, bool> cardBookList;
	public GameObject backButton;
	public GameObject nextButton;

	public GameObject unknownCardPrefab;

	private Dictionary<string, GameObject> cardMap;

	int pageLimit;
	int currentPage;
	private List<CardData> currentCardList;
	List<GameObject> dummyCardObjectList;
	public List<GameObject> dummyCardPrefabList;
	public TMP_Text cardCountText;
	ECardType selectedCardType = ECardType.None;


	public Dictionary<string, int> cardHashMap;

	int adventureCardCount = 0;
	int totalCardCount = 0;
	int serventCardCount = 0;
	int spellCardCount = 0;

	//카드깡
	void Gotcha()
	{
		Dictionary<ECardRarity, int> table = new Dictionary<ECardRarity, int>
		{	{ ECardRarity.Normal, 85 },
			{ ECardRarity.Rare, 15 }
		};

		ECardRarity selectedRarity = ECardRarity.None;

		List<CardData> selectedCardList = new();

		foreach(CardData cardData in cardDatabase)
		{
			if(selectedRarity == cardData.GetCardRarity())
			{selectedCardList.Add(cardData);}
		}

		CardData randomCard = selectedCardList[Random.Range(0, selectedCardList.Count)];
	}

	//카드도감
	void Awake()
	{

		cardHashMap = DataController.Inst.LoadCardHashMap();
		CreatePage();
	}

	public void SelectCardTypeNone()
	{ selectedCardType = ECardType.None; }
	public void SelectCardTypeServent()
	{ selectedCardType = ECardType.Servent; }

	public void SelectCardTypeSpell()
	{ selectedCardType = ECardType.Spell; }

	public void SelectCardTypeAdventure()
	{ selectedCardType = ECardType.Adventure; }

	public void InitiatePage()
	{
		currentPage = 0;
		UpdatePage();
	}

	public void CreatePage()
	{
		cardBookList = PlayerData.saveData.cardBookList;

		totalCardCount = cardDatabase.Count;

		foreach(CardData cardData in cardDatabase)
		{
			switch(cardData.GetCardType())
			{
				case ECardType.Servent:
					serventCardCount++;
					break;
				case ECardType.Spell:
					spellCardCount++;
					break;
				case ECardType.Adventure:
					adventureCardCount++;
					break;
				case ECardType.None:
					break;
			}
		}

		foreach(KeyValuePair<string, bool> valuePair in cardBookList)
		{
			//cardMap[valuePair.Key].GetComponent<DummyCard>().GetCardData().GetCardType();
		}
	}

	public void UpdatePage()
	{
		int count = 0;

		pageLimit = cardDatabase.Count / 6;
		int remainder = cardDatabase.Count % 6;

		currentCardList.Clear();
		foreach (GameObject gameObject in dummyCardObjectList)
		{ Destroy(gameObject); }

		if (currentPage != pageLimit)
		{ remainder = 6; }

		for (int i = 0; i < remainder; ++i)
		{ currentCardList.Add(cardDatabase[(currentPage * 6) + i]); }

		foreach (CardData  cardData in currentCardList)
		{
			GameObject cardObject = Instantiate(dummyCardPrefabList[cardHashMap[cardData.GetCardNum()]],
			new Vector3(0, 0, 0), Utils.QI);
			cardObject.transform.SetParent(cardLocations[count].transform);
			cardObject.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
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


		switch(selectedCardType)
		{
			case ECardType.None:
				cardCountText.text =  " / " + totalCardCount;
				break;

			case ECardType.Adventure:
				cardCountText.text = " / " + totalCardCount;
				break;

			case ECardType.Servent:
				cardCountText.text = " / " + totalCardCount;
				break;

			case ECardType.Spell:
				cardCountText.text = " / " + totalCardCount;
				break;
		}
	}


	//카드교환
}


[System.Serializable]
public class IndexedCard
{
	public string index;
	public GameObject cardObject;
}
