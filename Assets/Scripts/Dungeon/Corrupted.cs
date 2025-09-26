using System.Collections.Generic;
using BamaoUIPack.Scripts;

public class Corrupted : Dungeon
{
    public Corrupted()
    {
        dungeonName = "어두운 숲";
        dungeonNum = 0;
        dungeonEndFloor = 4;
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
    }

}