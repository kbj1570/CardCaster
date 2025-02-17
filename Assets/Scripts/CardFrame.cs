using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardFrame : MonoBehaviour, IPointerClickHandler
{
    bool clicked;
    int order;
    CardData cardData;
    public Image image;
    public TMP_Text cardCountText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardData(CardData cardData, int cardCount, int order)
    {
        this.cardData = cardData;
        cardCountText.text = cardCount.ToString();
        this.order = order;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
       DeckManager.Inst.AddCard(cardData, order); 
    }
}
