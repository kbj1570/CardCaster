using System.Collections.Generic;

public class UnknownMonster : Enemy
{
    public UnknownMonster()
    {
        enemyName = "정체불명의 괴물";
        enemyNum = "0";
        enemyHealth = 20;
        enemyGold = 300;
        actionToken = 4;

        enemyRewards = new Dictionary<Item, int>()
        {
            {new OldStick(), 4},
            {new DirtyPatch(), 4},
            {new SharpFang(), 4},
            {new ShardOfStarlight(), 2}
        };

        
        serventList = new List<CardData>
        {
            new ChaoticCorvus(),
            new Wild()
        };

        enemyAbility = new MysteriousFog();

        SetRewards();
    }
}