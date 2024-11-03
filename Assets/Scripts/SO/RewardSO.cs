using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardSO", menuName ="Scriptable Object/RewardSO")]

public class RewardSO : ScriptableObject
{
    public List<ItemSO> itemReward;
    public int cashReward;

    public List<ItemSO> GetItemReward()
    {return itemReward;}
    public int GetCashReward()
    {return cashReward;}
}