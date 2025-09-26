using System.Collections.Generic;
using BamaoUIPack.Scripts;

public class PrologueCorrupted : Dungeon
{
    public PrologueCorrupted()
    {
        dungeonName = "무너진 시가지";
        dungeonNum = 0;
        dungeonEndFloor = 2;
        dungeonFloorSize = 150;
        dungeonHeight = 10;
        dungeonWidth = 15;
        enemyLimit = 0;

        // floorDialogueMap = new Dictionary<int, int>()
        // {
        //     {2, 5}
        // };

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