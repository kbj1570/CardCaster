public class RedForest : Dungeon
{
    public RedForest()
    {
        randomCreate = true;
        dungeonName = "어두운 숲";
        dungeonNum = 0;
		dungeonEndFloor = 5;
        dungeonFloorSize = 100;
        dungeonHeight = 10;
        dungeonWidth = 10;
        enemyLimit = 1;

        itemList = new(){
            {new RedPotion(), 1},
        };

        safeFloorList = new(){
        };

        enemyList = new(){
            {new UnknownMonster(), 3}
        };

        dialogueList = new(){
		};

		maxGold = 10;
    }

}