using System.Collections.Generic;

public class UnknownMonster : Enemy
{
    public UnknownMonster()
    {
        enemyName = "정체불명의 자객";
        enemyNum = "0";
        enemyHealth = 40;
        enemyGold = 500;
        enemyRewards = new Dictionary<Item, int>
        {{ new GoldenDice(), 1 }};
        
        setventList = new List<CardData>
        {
            new ChaoticCorvus(),
            new Wolf()
        };
    }
}