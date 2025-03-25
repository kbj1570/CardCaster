public class Graveyard : Dungeon
{
    public Graveyard()
    {
        dungeonName = "옛 무덤";
        dungeonEndFloor = 5;
        dungeonFloorSize = 100;
        dungeonHeight = 10;
        dungeonWidth = 10;
        enemyLimit = 4;

        itemList = new(){
            {new RedPotion(), 1},
            {new GoldenDice(), 3},
            {new RustyKnife(), 2}
        };

        safeFloorList = new(){
            {8}
        };

        enemyList = new(){
            {new UnknownMonster(), 3}
        };

        encounterList = new(){
            {new MeetInTheDark(), 1}
        };

        maxGold = 200;
    }

}