using System.Collections.Generic;
using BamaoUIPack.Scripts;

public class RedForest : Dungeon
{
    public RedForest()
    {
        hasStaticFloor = true;
        dungeonName = "어두운 숲";
        dungeonNum = 0;
        dungeonEndFloor = 4;
        dungeonFloorSize = 100;
        dungeonHeight = 10;
        dungeonWidth = 10;
        enemyLimit = 0;

        // floorDialogueMap = new Dictionary<int, int>()
        // {
        //     {2, 5}
        // };

        staticFloors = new();

        List<ERoomType> tileType_1 = new()
        {
            ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EGold,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EStair,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EStart,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.EGold,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall
        };

        List<ERoomType> tileType_2 = new()
        {
            ERoomType.EGold,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.EItem,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.EStory,ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.None,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EStair,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EStart
        };
        List<ERoomType> tileType_3 = new()
        {
            ERoomType.EGold,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.EStory,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.None,
            ERoomType.EWall,ERoomType.EWall,ERoomType.EStory,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EStart,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.EItem,ERoomType.None
        };
        List<Node> nodes = new();


        for (int i = 0; i < tileType_1.Count; ++i)
        {

            nodes.Add(new Node());
            nodes[i].SetRoomType(tileType_1[i]);

            if (tileType_1[i] == ERoomType.EGold)
            { nodes[i].SetGold(30); }
            else if (tileType_1[i] == ERoomType.EStory)
            { nodes[i].SetDialogueNum(4); }
        }

        staticFloors.Add(nodes);

        nodes = new();
        for (int i = 0; i < tileType_2.Count; ++i)
        {

            nodes.Add(new Node());
            nodes[i].SetRoomType(tileType_2[i]);

            if (tileType_2[i] == ERoomType.EGold)
            { nodes[i].SetGold(30); }
            else if (tileType_2[i] == ERoomType.EItem)
            { nodes[i].SetItem(new RedPotion()); }
        }
        nodes[24].SetDialogueNum(5);
        nodes[42].SetDialogueNum(6);
        staticFloors.Add(nodes);

        nodes = new();
        for (int i = 0; i < tileType_3.Count; ++i)
        {

            nodes.Add(new Node());
            nodes[i].SetRoomType(tileType_3[i]);

            if (tileType_3[i] == ERoomType.EGold)
            { nodes[i].SetGold(30); }
            else if (tileType_3[i] == ERoomType.EStory)
            { nodes[i].SetDialogueNum(7); }
            else if (tileType_3[i] == ERoomType.EItem)
            { nodes[i].SetItem(new RedPotion()); }
        }
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
    }

}