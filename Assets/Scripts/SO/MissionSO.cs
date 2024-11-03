using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionSO", menuName ="Scriptable Object/MissionSO")]

public class MissionSO : ScriptableObject
{
    public string missonName;
    public string clientName;
    public string missionDescription;
    public Dictionary<ItemSO, int> requirement;
    public EMissionType missionType;
    public EMissionPlace missionPlace;
    public int reward;
    public bool isAccepted;

    public MissionSO()
    {isAccepted = false;}


    public string GetMissionName(){return missonName;}
    public string GetClientName(){return clientName;}
    public string GetMissionDescription(){return missionDescription;}
    public Dictionary<ItemSO, int> GetRequirement(){return requirement;}
    public int GetReward(){return reward;}
    public void SetAccepted(){isAccepted = !isAccepted;}
    public bool GetAccepted(){return isAccepted;}
    public EMissionType GetMissionType(){return missionType;}
    public EMissionPlace GetMissionPlace(){return missionPlace;}
}

public enum EMissionType
{Subdue, Rescue, Errand}


public enum EMissionPlace
{Market, GraveYard, Laboratory, Church, Abyss}
