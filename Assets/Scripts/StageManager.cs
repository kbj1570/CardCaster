using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
class StageManager : MonoBehaviour
{
    int mapLevel;
    [SerializeField] int mapHeight;
    [SerializeField] int mapWidth;
    [SerializeField] int safeFloor;
    [SerializeField] GameObject nodePrefab;
    [SerializeField] Transform nodeLeft;
    [SerializeField] Transform nodeRight;

    StageNode playerLocation;
    [SerializeField] List<List<StageNode>> map;
    List<List<Vector3>> nodePosition;

    void Start()
    {
        CreateMap();
        SetNodePosition();
        SetStageStair();
        //ShowMap();
    }
    void CreateMap()
    {
        GameObject gameObject = null;
        map = new();
        List<StageNode> startStage = new();
        map.Add(startStage);
        gameObject = Instantiate(nodePrefab, new(), Utils.QI);
        StageNode newNode = gameObject.GetComponent<StageNode>();
        newNode.SetStageLevel(0);
        startStage.Add(newNode);
        for(int i = 0; i < mapHeight - 2; ++i)
        {
            List<StageNode> stage = new();
            
            int rand = Random.Range(3, mapWidth + 1);

            if(i == safeFloor){rand = 1;}

            for(int q = 0; q < rand; ++q)
            {
                gameObject = Instantiate(nodePrefab, new(), Utils.QI);
                newNode = gameObject.GetComponent<StageNode>();
                newNode.SetStageLevel(i + 1);
                stage.Add(newNode);
            }

            map.Add(stage);
        }
        List<StageNode> endStage = new();
             
        map.Add(endStage);
        gameObject = Instantiate(nodePrefab, new(), Utils.QI);
        newNode = gameObject.GetComponent<StageNode>();
        newNode.SetStageLevel(mapHeight - 1);
        endStage.Add(newNode);
        
    }
    
    void SetStageStair()
    {
        for(int i = 0; i < mapHeight - 1; ++i)
        {
            int rand = Random.Range(0, map[i].Count);
            map[i][rand].SetStair(true);
        }
        
    }

    void SetupStageNode()
    {
        int rand = 0;
        int randRangeX = 0;
        int randRangeY = 0;
        switch(mapLevel)
        {
            case 1:
            randRangeX = 1;
            randRangeY = 21;
            break;

            case 2:
            randRangeX = 21;
            randRangeY = 41;
            break;

            case 3:
            randRangeX = 41;
            randRangeY = 61;
            break;

            case 4:
            randRangeX = 61;
            randRangeY = 81;
            break;

            case 5:
            randRangeX = 81;
            randRangeY = 101;
            break;
        }
        rand = Random.Range(randRangeX, randRangeY);
        foreach(List<StageNode> floor in map)
        {
            foreach(StageNode stageNode in floor)
            {

            }
        }



        
    }
    
    void ShowMap()
    {

        foreach(List<StageNode> nodeList in map)
        {
            foreach(StageNode node in nodeList)
            {
                node.ShowStatus();
            }
        }
        
    }

    void SetNodePosition()
    {
        
        float interval = 5.0f;
        Vector3 startPosition = new()
        {
            x = (nodeLeft.position.x + nodeRight.position.x) / 2
        };

    }

    void MoveStage(StageNode targetStage)
    {
        playerLocation = targetStage;
        targetStage.SetVisible(true);
    }
    public enum StageClassification
    {
        Battle, Save, Merchant, SoulMerchant
    }
}