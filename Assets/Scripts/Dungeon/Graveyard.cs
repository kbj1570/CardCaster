public class Graveyard : Dungeon
{
    public Graveyard()
    {
        dungeonName = "옛 무덤";
        dungeonEndFloor = 15;
        dungeonFloorSize = 140;
        dungeonHeight = 10;
        dungeonWidth = 14;

        itemList = new(){
            {new RedPotion(), 2},
            {new GoldenDice(), 2},
            {new RustyKnife(), 2},
            {new ShardOfStarlight(), 1},
            {new OldStick(), 4}
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