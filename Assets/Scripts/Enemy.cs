using System.Collections.Generic;

public class Enemy
{
    protected string enemyName;
    protected string enemyNum;
    protected int enemyHealth;

    protected Dictionary<Item, int> enemyRewards;
    protected int enemyGold;
    protected List<CardData> setventList;

    public string GetName()
    {return enemyName;}

    public string GetNum()
    {return enemyNum;}

    public int GetHealth()
    {return enemyHealth;}

    public Dictionary<Item, int> GetRewards()
    {return enemyRewards;}

    public int GetGold()
    {return enemyGold;}
    public List<CardData> GetServentList()
    {return setventList;}
}