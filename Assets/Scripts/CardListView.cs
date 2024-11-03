using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardListView : MonoBehaviour
{

    public bool isOpened;
    public GameObject cardButtonPrefab;
    public GameObject ViewportContent;

    public List<CardData> cardList;
    public static CardListView Inst{get; private set;}
    void Start()
    {
        ScaleZero();
        // this.cardList = CardManager.Inst.GetCardList();
        GameObject buttonObject = null;
        foreach(CardData cardData in cardList)
        {
            buttonObject = Instantiate(cardButtonPrefab, new Vector3() , Utils.QI);
            buttonObject.transform.SetParent(ViewportContent.transform);
            buttonObject.transform.localScale = new Vector3(1,1,1);
        }
    }

    void Awake()
    {
        
    }
    public void OnOff()
    {
        if(isOpened)
        {
            Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
            isOpened = false;
        }
        else
        {
            Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.InOutQuad);
            isOpened = true;
        }
    }

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;
}
