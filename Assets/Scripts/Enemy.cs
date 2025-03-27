using System;
using System.Collections.Generic;

public class Enemy
{
    protected string enemyName;
    protected string enemyNum;
    protected int enemyHealth;
    protected int actionToken;

    protected Dictionary<Item, int> enemyRewards;
    protected KeyValuePair<Item, int> rewards;
    protected int enemyGold;
    protected List<CardData> serventList;
    protected List<EnemyAbility> enemyAbilities;


    public string GetName()
    {return enemyName;}

    public string GetNum()
    {return enemyNum;}

    public int GetHealth()
    {return enemyHealth;}

    public Dictionary<Item, int> GetRewards()
    {return enemyRewards;}

    public KeyValuePair<Item, int> GetReward()
    {return rewards;}

    public int GetGold()
    {return enemyGold;}
    public List<CardData> GetServentList()
    {return serventList;}
    public List<EnemyAbility> GetEnemyAbilities()
    {return enemyAbilities;}
    public int GetActionToken()
    {return actionToken;}


    public void SetRewards()
    {
        Random random = new Random();
        int randomNum = random.Next(0, enemyGold / 5);

        if(random.Next(0, 2) == 0)
        {enemyGold += randomNum;}
        else
        {enemyGold -= randomNum;}

        if(enemyRewards != null)
        {   
            int count = 0;
            Dictionary<Item, int> rewardRoullet = new();

            foreach(KeyValuePair<Item, int> reward in enemyRewards)
            {
                count += reward.Value;
                rewardRoullet.Add(reward.Key, count);
            }

            randomNum = random.Next(0, count + 1);

            foreach(KeyValuePair<Item, int> reward in rewardRoullet)
            {
                if(randomNum <= reward.Value)
                {
                    randomNum = random.Next(1, 4);
                    rewards = new KeyValuePair<Item, int>(reward.Key, randomNum);
                    return;
                }
            }
        }
    }
}