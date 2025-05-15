using System.Collections.Generic;

public class SaveData
{
    public int health;
    public Dictionary<string, int> inventory;
	public Dictionary<string, int> storage;
	public int gold;
    public Dictionary<string, int> cardList;
    public Dictionary<string, int> deck;
    public ELocation currentLocation;
    public int currentFloor;
}

public enum ELocation
{None, Graveyard, Village, Campsite, Tent}