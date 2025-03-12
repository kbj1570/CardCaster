using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemAlert : Window
{
    public TMP_Text alertText;

    public void SetText(string itemName)
    {alertText.text = itemName +"을(를) 사용하시겠습니까?";}
}
