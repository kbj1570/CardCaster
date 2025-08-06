public class Graveyard : Dungeon
{
    public Graveyard()
    {
        dungeonName = "옛 무덤";
        dungeonEndFloor = 5;
        dungeonFloorSize = 140;
        dungeonHeight = 10;
        dungeonWidth = 14;
        enemyLimit = 3;

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

        randomEventList = new(){

        };

        maxGold = 20;
    }

}