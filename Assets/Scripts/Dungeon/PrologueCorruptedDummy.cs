using System.Collections.Generic;
using BamaoUIPack.Scripts;

public class PrologueCorruptedDummy : Dungeon
{
    public PrologueCorruptedDummy()
    {
        hasStaticFloor = true;
        dungeonName = "무너진 시가지";
        dungeonNum = 0;
        dungeonEndFloor = 2;
        dungeonFloorSize = 150;
        dungeonHeight = 10;
        dungeonWidth = 15;
        enemyLimit = 0;

        
        staticFloors = new();

        // floorDialogueMap = new Dictionary<int, int>()
        // {
        //     {2, 5}
        // };

        List<ERoomType> tileType = new()
        {
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.None,ERoomType.EStart,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EStory,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EStory,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
        };

        List<Node> nodes = new();


        for (int i = 0; i < tileType.Count; ++i)
        {

            nodes.Add(new Node());
            nodes[i].SetRoomType(tileType[i]);
        }
        nodes[109].SetDialogueNum(9);
        nodes[38].SetDialogueNum(10);
        
        staticFloors.Add(nodes);

        itemList = new(){
            {new RedPotion(), 1},
        };

        safeFloorList = new()
        {};

        enemyList = new(){
            {new UnknownMonster(), 3}
        };

        maxGold = 10;
        nextScene = "Chapter 2";
    }

}