using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
class StageNode : MonoBehaviour
{
    [SerializeField] int stageLevel;
    [SerializeField] int eventNum;
    [SerializeField] int nodeNum;
    [SerializeField] bool isVisible;
    [SerializeField] bool hasStair;

    public int GetStageLevel(){return stageLevel;}
    public void SetStageLevel(int value){stageLevel = value;}
    public void SetEventNum(int value){eventNum = value;}
    public void SetVisible(bool value){isVisible = value;}
    public void SetStair(bool value){hasStair = value;}

    public void ShowStatus()
    {
        print("StageLevel: " + stageLevel);
        print("eventNum: " + eventNum);
        print("isVisible: " + isVisible);
        print("hasChair: " + hasStair);
    }

}