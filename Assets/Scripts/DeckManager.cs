using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
public class DeckManager : MonoBehaviour
{
    public List<Transform> cardLocation;
    public GameObject cardPrefab;
    public GameObject smallCardPrefab;
    public GridLayoutGroup gridLayout;
    private Dictionary<CardData, int> myCardList;
    private Dictionary<CardData, int> myDeckList;
    private List<CardData> cardDatabase;

    public List<GameObject> deckSettingCardObjectList;
    public List<GameObject> deckCardObjectList;

    private Dictionary<CardData, int> currentCardList;
    private int currentPage;
    private int pageLimit;
    public static DeckManager Inst{get; private set;}
    void Awake() => Inst = this;

    void Start()
    {
        currentPage = 0;
        cardDatabase = DataController.Inst.LoadCardDatabase();

        // LoadCardList();
        // LoadDeck();
        // CreatePage();
    }

    public void SaveDeck()
    {
        Dictionary<string, int> dumb = new Dictionary<string, int>();
        foreach(KeyValuePair<CardData, int> value in myDeckList)
        {dumb.Add(value.Key.GetCardNum().ToString(), value.Value);}

        DataController.Inst.SaveDeck(dumb);
    }
    public void LoadDeck()
    {
       Dictionary<CardData, int> dumb = new Dictionary<CardData, int>();

        foreach(KeyValuePair<string, int> value in DataController.Inst.LoadDeck())
        {dumb.Add(cardDatabase[Convert.ToInt32(value.Key)], value.Value);}

        myDeckList = dumb;
    }
    public void SaveCardList()
    {
        Dictionary<string, int> dumb = new Dictionary<string, int>();

        

        foreach(KeyValuePair<CardData, int> value in myCardList)
        {dumb.Add(value.Key.GetCardNum().ToString(), value.Value);}

        DataController.Inst.SaveCardList(dumb);
    }
    public void LoadCardList()
    {
        Dictionary<CardData, int> dumb = new Dictionary<CardData, int>();

        foreach(KeyValuePair<string, int> value in DataController.Inst.LoadCardList())
        {dumb.Add(cardDatabase[Convert.ToInt32(value.Key)], value.Value);}

        myCardList = dumb;
    }
    public void CreatePage()
    {
        // for(int i = 0; i < cardLocation.Count; ++i)
        // {
        //     deckSettingCardObjectList.Add(Instantiate(cardPrefab, new Vector3(0,0,0) , Utils.QI));
        //     deckSettingCardObjectList[i].transform.SetParent(cardLocation[i].transform);
        //     deckSettingCardObjectList[i].transform.localScale = new Vector3(1,1,1);
        // }
        currentCardList = new Dictionary<CardData, int>();

        UpdatePage();
        UpdateDeckPage();
    }

    public void UpdatePage()
    {

        pageLimit = myCardList.Count / 6;
        int remainder = myCardList.Count % 6;

        currentCardList.Clear();

        List<CardData> cardList = new List<CardData>(myCardList.Keys);

        if(currentPage != pageLimit)
        {remainder = 6;}

        for(int i = 0; i < remainder; ++i)
        {currentCardList.Add(cardList[(currentPage * 6) + i], myCardList[cardList[(currentPage * 6) + i]]);}

        // foreach(KeyValuePair<CardData, int> value in myCardList)
        // {currentCardList.Add(value.Key, value.Value);}


        foreach(GameObject gameObject in deckSettingCardObjectList)
        {gameObject.SetActive(true);}

        for(int i = 0; i < currentCardList.Count; ++i)
        {deckSettingCardObjectList[i].GetComponent<DeckSettingCard>().UpdateCard(currentCardList.ToList()[i].Key, currentCardList.ToList()[i].Value);}

        if(currentCardList.Count != 6)
        {
            for(int i = 5; i > currentCardList.Count - 1; --i)
            {deckSettingCardObjectList[i].SetActive(false);}
        }

        
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
        foreach(GameObject gameObject in deckCardObjectList)
        {Destroy(gameObject);}

        foreach(KeyValuePair<CardData, int> value in myDeckList)
        {
            GameObject gameObject = Instantiate(smallCardPrefab, new Vector3(0,0,0) , Utils.QI);
            deckCardObjectList.Add(gameObject);

            gameObject.transform.SetParent(gridLayout.transform);
            gameObject.GetComponent<DeckCard>().SetCard(value.Key, value.Value);
        }
    }

    public void AddCard(CardData value)
    {
        if(!myDeckList.ContainsKey(value))
        {myDeckList.Add(value, 1);}
        else
        {myDeckList[value]++;}

        myCardList[value]--;

        if(myCardList[value] == 0)
        {myCardList.Remove(value);}

        UpdatePage();
        UpdateDeckPage();
    }

    public void DeleteCard(CardData value)
    {
        myDeckList[value]--;

        if(myDeckList[value] == 0)
        {myDeckList.Remove(value);}

        if(!myCardList.ContainsKey(value))
        {myCardList.Add(value, 1);}
        else
        {myCardList[value]++;}

        UpdatePage();
        UpdateDeckPage();
    }

}