using System.Collections.Generic;

public class Dungeon
{
    protected bool randomCreate;
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
    protected List<RandomEvent> bannedEncounterList;
	protected List<int> dialogueList;
	public string GetDungeonName()
    {return dungeonName;}
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

    public Dictionary<int, string> GetSafeFloorList()
    {return safeFloorList;}
    public Dictionary<Enemy, int> GetEnemyList()
    {return enemyList;}

    public Dictionary<ItemData, int> GetItemList()
    {return itemList;}

    public Dictionary<RandomEvent, int> GetRandomEventList()
    {return randomEventList;}
    public List<RandomEvent> GetBannedEncounterList()
    {return bannedEncounterList;}

	public List<int> GetDialogueList()
	{ return dialogueList; }


}