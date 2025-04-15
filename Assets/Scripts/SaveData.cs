using System.Collections.Generic;

public class SaveData
{
    public int health;
    public List<string> inventory;
    public int gold;
    public Dictionary<string, int> cardList;
    public Dictionary<string, int> deck;
    public ELocation currentLocation;
    public int currentFloor;
}

public enum ELocation
{None, Graveyard, Village, Campsite, Tent}