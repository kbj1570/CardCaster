public class Player
{
    private int health;
    private int gold;


    public int GetHealth()
    {return health;}

    public void SetHealth(int value)
    {health = value;}

    public int GetGold()
    {return gold;}

    public void SetGold(int value)
    {gold = value;}

    public void GainItem(Item item)
    {}
}