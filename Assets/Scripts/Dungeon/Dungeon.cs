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
    protected List<int> safeFloorList;
    protected Dictionary<Encounter, int> encounterList;
    protected List<Encounter> bannedEncounterList;
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

    public List<int> GetSafeFloorList()
    {return safeFloorList;}
    public Dictionary<Enemy, int> GetEnemyList()
    {return enemyList;}

    public Dictionary<Item, int> GetItemList()
    {return itemList;}

    public Dictionary<Encounter, int> GetEncounterList()
    {return encounterList;}
    public List<Encounter> GetBannedEncounterList()
    {return bannedEncounterList;}


}