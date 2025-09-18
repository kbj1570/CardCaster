using System.Collections.Generic;
using NUnit.Framework;

public class Dungeon
{
    protected bool hasStaticFloor;
    protected string dungeonName;
    protected int dungeonNum;
    protected int dungeonEndFloor;
    protected int dungeonFloorSize;
    protected int dungeonWidth;
    protected int dungeonHeight;
    protected int maxGold;
    protected int enemyLimit;

    protected Dictionary<Enemy, int> enemyList;
    protected Dictionary<ItemData, int> itemList;
	protected Dictionary<int, string> safeFloorList;
	protected Dictionary<RandomEvent, int> randomEventList;
    protected Dictionary<int, int> floorDialogueMap;
    protected List<RandomEvent> bannedEncounterList;
	protected List<int> dialogueList;
    protected List<List<Node>> staticFloors;
	public string GetDungeonName()
    { return dungeonName; }
    public int GetDungeonNum()
    { return dungeonNum; }

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
    
    public bool GetHasStaticFloor()
    { return hasStaticFloor; }
    public List<List<Node>> GetStaticFloors()
    { return staticFloors; }

    public Dictionary<int, string> GetSafeFloorList()
    { return safeFloorList; }
    public Dictionary<Enemy, int> GetEnemyList()
    {return enemyList;}

    public Dictionary<ItemData, int> GetItemList()
    {return itemList;}
    
    public Dictionary<int, int> GetFloorDialogueMap()
    { return floorDialogueMap; }

	public List<int> GetDialogueList()
    { return dialogueList; }


}