using System.Collections;
using System.Collections.Generic;

public class UnknownMonster : Enemy
{
    public UnknownMonster()
    {
        enemyName = "정체불명의 괴물";
        enemyNum = "0";
        enemyHealth = 3;
        enemyGold = 70;
        actionToken = 3;

        enemyRewards = new Dictionary<ItemData, int>()
        {
            {new RedPotion(), 5},
			{new ShardOfStarlight(), 5}
		};

        
        serventList = new List<EnemyServentCardData>
        {
            new ShadowCorvus(),
            new Wild()
        };

        SetRewards();
    }

	public IEnumerator EffectExecute(BattleManager bm)
	{
		yield return null;
	}
}