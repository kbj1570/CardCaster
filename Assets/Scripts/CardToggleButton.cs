using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardToggleButton : MonoBehaviour
{
    public CardData cardDataSO;
    public TMP_Text cardNameText;
    public Toggle toggle;
    public GameObject check;

    void Start()
    {
        ColorBlock colorBlock = toggle.colors;
        colorBlock.normalColor = new Color(0.207f, 0.691f, 0.488f, 1f);
        colorBlock.selectedColor  = new Color(0.207f, 0.691f, 0.688f, 1f);
        colorBlock.pressedColor  = new Color(0.207f, 0.691f, 0.888f, 1f);

        toggle.colors = colorBlock;
        check.SetActive(false);
    }

    public void CheckOnOff()
    {
        if(toggle.isOn)
        {
            check.SetActive(true);
        }
        else
        {
            check.SetActive(false);
        }
    }
    public Toggle GetToggle(){return toggle;}

    public void SetCardData(CardData value)
    {
        this.cardDataSO = value;
        cardNameText.text = cardDataSO.GetCardName();
    }

    public CardData GetCardDataSO(){return cardDataSO;}
}
