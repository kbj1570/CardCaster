using System.Collections.Generic;

public class CrimsonCrow : Enemy
{
	int specialActionHealth = 15;
	public CrimsonCrow()
	{
		enemyName = "°Å´ë±î¸¶±Í";
		enemyNum = "1";
		enemyHealth = 30;
		enemyGold = 500;
		actionToken = 4;
		enemyRewards = new Dictionary<ItemData, int>()
		{
			{new ShardOfStarlight(), 3},
			{new RedPotion(), 10}
		};
		serventList = new List<EnemyServentCardData>
		{
			new ChaoticCorvus(),
			new Wild()
		};
		SetRewards();
	}
}