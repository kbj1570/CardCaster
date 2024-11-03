using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class ItemListView : MonoBehaviour
{

    [SerializeField] bool isOpened;
    [SerializeField] GameObject cardButtonPrefab;
    [SerializeField] GameObject ViewportContent;

    public Dictionary<CardData, int> cardList;


    private List<GameObject> contentList;

    public static ItemListView Inst {get; private set;}
    void Awake() => Inst = this;
    void Start()
    {
        ScaleOne();
        // this.cardList = CardManager.Inst.GetCardList();
        contentList = new List<GameObject>();
        OnOff();
    }


    public void OnOff()
    {
        if(isOpened)
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
            isOpened = false;
        }
        else
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.InOutQuad);
            isOpened = true;
        }
    }

    public void Clear()
    {
        foreach(GameObject value in contentList)
        {
            Destroy(value);
        }
        contentList.Clear();
    }

    public void ShowTradingMenu()
    {
        GameObject buttonObject = null;
        foreach(KeyValuePair<CardData, int> data in cardList)
        {
            buttonObject = Instantiate(cardButtonPrefab, new Vector3() , Utils.QI);
            buttonObject.GetComponent<TradingCard>().SetCardData(data.Key);
            buttonObject.GetComponent<TradingCard>().UpdateStatus(data.Value, 10);
            buttonObject.transform.SetParent(ViewportContent.transform);
            buttonObject.transform.localScale = new Vector3(1,1,1);
            contentList.Add(buttonObject);
        }   
    }

    public void SetCardData(Dictionary<CardData, int> value)
    {
        this.cardList = value;
    }

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;
}
