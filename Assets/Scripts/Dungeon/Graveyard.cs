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

        enemyList = new(){
            {new UnknownMonster(), 3}
        };

        dialogueList = new(){
            1
		};

		maxGold = 20;
    }

}