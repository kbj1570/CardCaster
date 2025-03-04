using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SideKickManager : MonoBehaviour
{

    [SerializeField]List<DialogueNode> dialogueList;
    [SerializeField]List<CardData> fullCardList;
    [SerializeField]List<CardData> enrolledCardList;
    [SerializeField]List<CardData> sideCardList;
    [SerializeField]GameObject viewportContent;
    public List<CardData> tradeCardList;
    public GameObject cardButtonPrefab;
    public int dialogueSequence;

    public GameObject tradingCard_1;
    public GameObject tradingCard_2;
    public GameObject tradingCard_3;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void SetDialogueSequence(int value)
    {dialogueSequence = value;}

    public int GetDialogueSequence()
    {return dialogueSequence;}

    public void BackToSafeZone()
    {
        SceneManager.LoadScene("SafeZone");
    }

    public void ShowTradingMenu()
    {
        GameObject buttonObject_1 = null;
        GameObject buttonObject_2 = null;
        GameObject buttonObject_3 = null;
        // foreach(CardDataSO cardData in tradeCardList)
        // {
        //     buttonObject = Instantiate(cardButtonPrefab, new Vector3() , Utils.QI);
        //     buttonObject.transform.SetParent(viewportContent.transform);

        //     buttonObject.GetComponent<RectTransform>().sizeDelta = tradingCard_1.GetComponent<RectTransform>().sizeDelta;
        //     buttonObject.transform.position = tradingCard_1.transform.position;
        //     buttonObject.GetComponent<CardButton>().Setup(cardData);
        //     buttonObject.transform.localScale = new Vector3(1,1,1);
        // }

        buttonObject_1 = Instantiate(cardButtonPrefab, new Vector3() , Utils.QI);
        buttonObject_2 = Instantiate(cardButtonPrefab, new Vector3() , Utils.QI);
        buttonObject_3 = Instantiate(cardButtonPrefab, new Vector3() , Utils.QI);

        buttonObject_1.transform.SetParent(viewportContent.transform);
        buttonObject_2.transform.SetParent(viewportContent.transform);
        buttonObject_3.transform.SetParent(viewportContent.transform);

        buttonObject_1.GetComponent<RectTransform>().sizeDelta = tradingCard_1.GetComponent<RectTransform>().sizeDelta;
        buttonObject_2.GetComponent<RectTransform>().sizeDelta = tradingCard_2.GetComponent<RectTransform>().sizeDelta;
        buttonObject_3.GetComponent<RectTransform>().sizeDelta = tradingCard_3.GetComponent<RectTransform>().sizeDelta;

        buttonObject_1.transform.position = tradingCard_1.transform.position;
        buttonObject_2.transform.position = tradingCard_2.transform.position;
        buttonObject_3.transform.position = tradingCard_3.transform.position;


        buttonObject_1.GetComponent<CardButton>().Setup(tradeCardList[0]);
        buttonObject_2.GetComponent<CardButton>().Setup(tradeCardList[1]);
        buttonObject_3.GetComponent<CardButton>().Setup(tradeCardList[2]);

        buttonObject_1.transform.localScale = new Vector3(1,1,1);
        buttonObject_2.transform.localScale = new Vector3(1,1,1);
        buttonObject_3.transform.localScale = new Vector3(1,1,1);   
    }

    public void ShowRequirements(int value)
    {
        Dictionary<CardData, int> c = new Dictionary<CardData, int>();

        List<CardData> cards = null;
        // List<CardDataSO> cards = tradeCardList[value].GetRequirements();

        foreach(CardData cardData in cards)
        {
            if(!c.ContainsKey(cardData))
            {c.Add(cardData, 1);}
            else
            {c[cardData]++;}
        }
        ItemListView.Inst.SetCardData(c);
        ItemListView.Inst.ShowTradingMenu();
    }
}
