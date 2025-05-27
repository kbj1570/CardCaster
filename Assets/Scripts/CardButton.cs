using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class CardButton : MonoBehaviour
{

    public Image card;
    public Image character;
    public TMP_Text nameTMP;
    public TMP_Text healthTMP;
    public TMP_Text descriptionTMP;
    public TMP_Text costTMP;
    public Sprite cardFront;
    public Sprite cardBack;

    public CardData cardData;
    int currentCost;
    public PRS originPRS;
    public CardData GetCardData(){return cardData;}
    
    public void Setup(CardData cardData)
    {
        //this.cardData = cardData;
        //// character.sprite = this.cardData.GetSprite();
        //nameTMP.text = this.cardData.GetCardName();
        //if(cardData.GetCardType() == ECardType.Servent)
        //    healthTMP.text = this.cardData.GetForce().ToString();
        
        //descriptionTMP.text = this.cardData.GetCardAbility();
        //costTMP.text = this.cardData.GetCardCost().ToString();
        //currentCost = this.cardData.GetCardCost();
    }
}
