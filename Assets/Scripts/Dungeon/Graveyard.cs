public class Graveyard : Dungeon
{
    public Graveyard()
    {
        dungeonName = "옛 무덤";
        dungeonEndFloor = 5;
        dungeonFloorSize = 300;
        dungeonHeight = 15;
        dungeonWidth = 20;

        itemList = new(){
            {new RedPotion(), 2},
            {new GoldenDice(), 2},
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