using System.Collections.Generic;

public class UnknownMonster : Enemy
{
    public UnknownMonster()
    {
        enemyName = "정체불명의 괴물";
        enemyNum = "0";
        enemyHealth = 10;
        enemyGold = 300;
        actionToken = 3;

        enemyRewards = new Dictionary<Item, int>()
        {
            {new ShardOfStarlight(), 2}
        };

        
        serventList = new List<EnemyServentCardData>
        {
            new ChaoticCorvus(),
            new Wild()
        };

        enemyAbility = new MysteriousFog();

        SetRewards();
    }
}