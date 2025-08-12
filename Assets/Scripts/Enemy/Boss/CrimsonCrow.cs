using System.Collections.Generic;

public class CrimsonCrow : Enemy
{
	public CrimsonCrow()
	{
		enemyName = "°Å´ë±î¸¶±Í";
		enemyNum = "1";
		enemyHealth = 15;
		enemyGold = 500;
		actionToken = 4;
		enemyRewards = new Dictionary<Item, int>()
		{
			{new ShardOfStarlight(), 3}
		};
		serventList = new List<EnemyServentCardData>
		{
			new ChaoticCorvus(),
			new Wild()
		};
		SetRewards();
	}
}