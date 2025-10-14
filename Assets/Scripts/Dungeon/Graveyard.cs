using System.Collections.Generic;

public class Graveyard : Dungeon
{
    public Graveyard()
    {
        dungeonName = "공동묘지";
        dungeonNum = 1;
		dungeonEndFloor = 5;
        dungeonFloorSize = 120;
        dungeonHeight = 10;
        dungeonWidth = 12;
        enemyLimit = 1;

        itemList = new(){
            {new RedPotion(), 1},
            {new GoldenDice(), 3},
            {new RustyKnife(), 2}
        };

        safeFloorList = new(){
            {1,"SafeZone_Graveyard"}
        };


        List<ERoomType> tileType = new()
        {
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,
            ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None
        };

        List<Node> nodes = new();
        for (int i = 0; i < tileType.Count; ++i)
        {
            nodes.Add(new Node());
            nodes[i].SetRoomType(tileType[i]);
        }

        safeFloor = nodes;

        enemyList = new(){
            {new UnknownMonster(), 3}
        };

        dialogueList = new(){
            1
		};

		maxGold = 20;
    }

}