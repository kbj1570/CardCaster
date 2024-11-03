using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardSelectView : MonoBehaviour
{

    public bool isOpened;
    public GameObject cardButtonPrefab;
    public GameObject viewportContent;
    List<GameObject> buttonList;

    public List<CardData> cardList;
    public static CardListView Inst{get; private set;}
    void Start()
    {
        ScaleZero();
        // this.cardList = CardManager.Inst.GetCardList();
        GameObject buttonObject = null;
        buttonList = new List<GameObject>();
        foreach(CardData cardData in cardList)
        {
            buttonObject = Instantiate(cardButtonPrefab, new Vector3() , Utils.QI);
            buttonObject.GetComponent<CardToggleButton>().SetCardData(cardData);
            buttonObject.transform.SetParent(viewportContent.transform);
            buttonObject.transform.localScale = new Vector3(1,1,1);

            buttonList.Add(buttonObject);
        }
    }

    public void move()
    {
        foreach(GameObject button in buttonList)
        {
            if(button.GetComponent<CardToggleButton>().GetToggle().isOn)
            {
                CardManager.Inst.AddSelectedCard(button.GetComponent<CardToggleButton>().GetCardDataSO());
            }
            
        }
    }

    void Update()
    {

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

