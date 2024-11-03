using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TradingCard : MonoBehaviour
{
    public CardData cardData;
    public TMP_Text cardNameText;
    public TMP_Text cardCountText;

    public int cardCount;
    public int cardDemand;
    public bool canTrade;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void UpdateStatus(int cardDemandValue, int cardCountValue)
    {
        cardCount = cardCountValue;
        cardDemand = cardDemandValue;
        cardNameText.text = cardData.GetCardName();
        cardCountText.text = cardDemand + " / " + cardCount;
    }

    public void SetCardData(CardData value)
    {this.cardData = value;}
}
