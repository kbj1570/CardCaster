using System.Collections.Generic;

public class UnknownMonster : Enemy
{
    public UnknownMonster()
    {
        enemyName = "정체불명의 자객";
        enemyNum = "0";
        enemyHealth = 40;
        enemyGold = 300;
        enemyRewards = new Dictionary<Item, int>
        {{new OldStick(), 4},
         {new DirtyPatch(), 4},
         {new SharpFang(), 2},
         {new ShardOfStarlight(), 1}};
        
        serventList = new List<CardData>
        {
            new ChaoticCorvus(),
            new Wild()
        };

        enemyAbilities = new List<EnemyAbility>
        {
            new MysteriousFog()
        };

        SetRewards();
    }
}