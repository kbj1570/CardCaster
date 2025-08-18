using System.Collections.Generic;

public class SaveData
{
    public int health;
    public List<string> inventory_items;
	public List<string> storage_items;
	public int gold;
    public int shard;
    public Dictionary<string, int> cardList;
	public Dictionary<string, bool> cardArchiveList;
	public Dictionary<string, int> deck;
    public Dictionary<string, bool> cutsceneWatched;
    public ELocation currentLocation;
    public int currentFloor;
}

public enum ELocation
{None, Graveyard, Village, Campsite, Tent}