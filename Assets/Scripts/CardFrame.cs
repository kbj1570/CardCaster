using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardFrame : MonoBehaviour, IPointerClickHandler
{
    bool locked;
    int order;
    CardData cardData;
    public Image image;
    public TMP_Text cardCountText;


    public void SetCardData(CardData cardData, int cardCount, int order, bool locked)
    {
        this.cardData = cardData;
        cardCountText.text = cardCount.ToString();
        this.order = order;
        this.locked = locked;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!locked)
        DeckManager.Inst.AddCard(cardData, order);
        else
        DeckManager.Inst.AlertError();
    }
}
