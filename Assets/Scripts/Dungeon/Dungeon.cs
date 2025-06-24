using System.Collections.Generic;

public class Dungeon
{
    protected string dungeonName;
    protected int dungeonEndFloor;
    protected int dungeonFloorSize;
    protected int dungeonWidth;
    protected int dungeonHeight;
    protected int maxGold;
    protected int enemyLimit;

    protected Dictionary<Enemy, int> enemyList;
    protected Dictionary<Item, int> itemList;
	protected Dictionary<int, string> safeFloorList;
	protected Dictionary<RandomEvent, int> encounterList;
    protected List<RandomEvent> bannedEncounterList;
    public string GetDungeonName()
    {return dungeonName;}

    public int GetDungeonEndFloor()
    {return dungeonEndFloor;}

    public int GetDungeonFloorSize()
    {return dungeonFloorSize;}

    public int GetDungeonWidth()
    {return dungeonWidth;}

    public int GetDungeonHeight()
    {return dungeonHeight;}

    public int GetMaxGold()
    {return maxGold;}

    public int GetEnemyLimit()
    {return enemyLimit;}

    public Dictionary<int, string> GetSafeFloorList()
    {return safeFloorList;}
    public Dictionary<Enemy, int> GetEnemyList()
    {return enemyList;}

    public Dictionary<Item, int> GetItemList()
    {return itemList;}

    public Dictionary<RandomEvent, int> GetEncounterList()
    {return encounterList;}
    public List<RandomEvent> GetBannedEncounterList()
    {return bannedEncounterList;}


}