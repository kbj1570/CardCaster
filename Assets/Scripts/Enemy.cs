using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public enum EnemyType
{
    Normal,
    Elite,
    Boss
}
public class Enemy
{
    protected string enemyName;
    protected string enemyNum;
    protected int enemyHealth;
    protected int actionToken;

    protected Dictionary<ItemData, int> enemyRewards;
    protected List<ItemData> rewards;
    protected int enemyGold;
    protected List<EnemyServentCardData> serventDeck;


	public string GetName()
    {return enemyName;}

    public string GetNum()
    {return enemyNum;}

    public int GetHealth()
    {return enemyHealth;}


    public List<ItemData> GetReward()
    {return rewards;}
    public int GetGold()
    {return enemyGold;}

    
    public List<EnemyServentCardData> GetServentDeck()
    {return serventDeck;}
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

		List<ItemData> rewardList = new List<ItemData>();

		int rewardCount = random.Next(0, 4);

		if (rewardCount == 0 || enemyRewards.Count == 0)
        {
			rewards = rewardList;
			return;
		}
			
		int totalWeight = enemyRewards.Values.Sum();

		for (int i = 0; i < rewardCount; i++)
		{
			int roll = random.Next(0, totalWeight);
			int cumulative = 0;

			foreach (var kvp in enemyRewards)
			{
				cumulative += kvp.Value;
				if (roll < cumulative)
				{
					rewardList.Add(kvp.Key);
					break;
				}
			}
		}

		rewards = rewardList;
	}

	public IEnumerator EffectExecute(BattleManager bm)
	{
		yield return null;
	}
}
