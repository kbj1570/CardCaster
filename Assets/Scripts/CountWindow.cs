using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountWindow : Window
{

    int countLimit;
    int countValue;
    public TMP_Text countText;
    public void SetLimit(int value)
    {countLimit = value;}

    public void CountUp()
    {
        if(countValue < countLimit && countValue >= 1)
        {countValue++;}
    }

    public void CountDown()
    {
        if(countValue <= countLimit && countValue > 1)
        {countValue--;}
    }

    public void UpdateCountText()
    {countText.text = countValue.ToString();}

    public void SetCountLimit(int value)
    {countLimit = value;}
    public int GetCount()
    {return countValue;}

    public void SetCount(int value)
    {this.countValue = value;}
}
