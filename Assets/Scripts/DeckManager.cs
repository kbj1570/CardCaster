using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using TMPro;
public class DeckManager : MonoBehaviour
{
    public Transform gridlayoutPosition;
    public List<Transform> cardLocation;
    public GameObject window;
    public GameObject cardPrefab;
    public GameObject smallCardPrefab;
    public GameObject cardFrame;
    public GridLayoutGroup gridLayout;
    private Dictionary<CardData, int> myCardList;
    private Dictionary<CardData, int> myDeckList;
    private List<CardData> cardDatabase;
    private List<CardData> currentPageCardList;
    public List<GameObject> dummyCardObjectList;
    public List<GameObject> dummyCardPrefabList;
    private List<GameObject> deckCardObjectList;

    private Dictionary<CardData, int> currentCardList;
    private int currentPage;
    private int pageLimit;
    public static DeckManager Inst{get; private set;}
    public TMP_Text pageNumber;
    public TMP_Text searchText;
    void Awake() => Inst = this;

    void Start()
    {
        currentPage = 0;
        cardDatabase = DataController.Inst.LoadCardDatabase();
        myCardList = new();
        deckCardObjectList = new();

        LoadCardList();
        LoadDeck();
        CreatePage();
    }

    public void ResetCardList()
    {

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
        foreach(KeyValuePair<string, int> value in DataController.Inst.LoadCardList())
        {myCardList.Add(cardDatabase[Convert.ToInt32(value.Key)], value.Value);}
    }
    public void CreatePage()
    {
        // for(int i = 0; i < cardLocation.Count; ++i)
        // {
        //     dummyCardObjectList.Add(Instantiate(cardPrefab, new Vector3(0,0,0) , Utils.QI));
        //     dummyCardObjectList[i].transform.SetParent(cardLocation[i].transform);
        //     dummyCardObjectList[i].transform.localScale = new Vector3(1,1,1);
        // }
        currentCardList = new Dictionary<CardData, int>();

        UpdatePage();
        UpdateDeckPage();
    }

    public void UpdatePage()
    {
        int count = 0;

        pageLimit = myCardList.Count / 6;
        int remainder = myCardList.Count % 6;

        currentCardList.Clear();
        foreach(GameObject gameObject in dummyCardObjectList)
        {Destroy(gameObject);}

        List<CardData> cardList = new List<CardData>(myCardList.Keys);

        if(currentPage != pageLimit)
        {remainder = 6;}

        for(int i = 0; i < remainder; ++i)
        {currentCardList.Add(cardList[(currentPage * 6) + i], myCardList[cardList[(currentPage * 6) + i]]);}

        foreach(KeyValuePair<CardData, int> item in currentCardList)
        {
            GameObject cardObject = Instantiate(dummyCardPrefabList[item.Key.GetCardNum()],
            new Vector3(0,0,0) , Utils.QI);
            cardObject.transform.SetParent(cardLocation[count].transform);
            cardObject.transform.localScale = new Vector3(0.55f,0.55f,0.55f);
            cardObject.transform.localPosition = new Vector3(0,0,0);
            
            dummyCardObjectList.Add(cardObject);

            GameObject cardFrameObject = Instantiate(cardFrame,new Vector3(0,0,0) , Utils.QI);
            cardFrameObject.transform.SetParent(cardLocation[count].transform);
            cardFrameObject.transform.localPosition = new Vector3(0,0,0);
            cardFrameObject.GetComponent<CardFrame>().
            SetCardData(item.Key, item.Value, count);

            dummyCardObjectList.Add(cardFrameObject);


            count++;
        }

        pageNumber.text = (currentPage + 1) + " / " + (pageLimit + 1);

        // foreach(KeyValuePair<CardData, int> value in myCardList)
        // {currentCardList.Add(value.Key, value.Value);}




        // for(int i = 0; i < currentCardList.Count; ++i)
        // {dummyCardObjectList[i].GetComponent<DummyCard>().UpdateCard(currentCardList.ToList()[i].Key, currentCardList.ToList()[i].Value);}

        // if(currentCardList.Count != 6)
        // {
        //     for(int i = 5; i > currentCardList.Count - 1; --i)
        //     {dummyCardObjectList[i].SetActive(false);}
        // }

        
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

    public void AddCard(CardData value, int order)
    {
        if(!myDeckList.ContainsKey(value))
        {myDeckList.Add(value, 1);}
        else
        {myDeckList[value]++;}

        myCardList[value]--;

        if(myCardList[value] == 0)
        {myCardList.Remove(value);}

        GameObject gameObject = Instantiate(dummyCardPrefabList[value.GetCardNum()], cardLocation[order].position , Utils.QI);
        gameObject.transform.SetParent(window.transform);
        gameObject.GetComponent<DummyCard>().StartMoveAndScale(gridlayoutPosition.position);
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

        GameObject gameObject = Instantiate(dummyCardPrefabList[value.GetCardNum()], gridlayoutPosition.position , Utils.QI);
        gameObject.transform.SetParent(window.transform);
        gameObject.GetComponent<DummyCard>().StartMoveAndScale(window.transform.position);

        UpdatePage();
        UpdateDeckPage();
    }

}