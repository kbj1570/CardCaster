using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeckCard : MonoBehaviour
{
    private CardData cardData;
    private string cardName;
    private int count;
    public TMP_Text cardNameText;
    public TMP_Text cardCountText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetCard(CardData value, int count)
    {
        cardData = value;
        this.count = count;
        cardNameText.text = value.GetCardName();
        cardCountText.text = count.ToString();
    }
}
