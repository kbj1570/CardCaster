using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class DeckSettingCard : MonoBehaviour
{
    public TMP_Text cardPower;
    public UnityEngine.UI.Image cardCost;
    public UnityEngine.UI.Image cardImage;
    public Sprite oneStar;
    public Sprite twoStar;
    public Sprite threeStar;
    public Sprite fourStar;
    public TMP_Text cardText;
    public TMP_Text cardName;
    public TMP_Text cardCount;
    public CardData cardData;
    public void UpdateCard(CardData value, int count)
    {
        cardData = value;
        cardName.text = cardData.GetCardName();
        cardPower.text = cardData.GetForce().ToString();
        cardText.text = cardData.GetCardAbility();

        switch(cardData.GetCardCost())
        {
            case 0:
            cardCost.sprite = null;
            break;

            case 1:
            cardCost.sprite = oneStar;
            break;

            case 2:
            cardCost.sprite = twoStar;
            break;

            case 3:
            cardCost.sprite = threeStar;
            break;
        }
        cardCount.text = count.ToString();
    }

    public void CountUp()
    {DeckManager.Inst.AddCard(cardData);}

    public void CountDown()
    {DeckManager.Inst.DeleteCard(cardData);}
}
