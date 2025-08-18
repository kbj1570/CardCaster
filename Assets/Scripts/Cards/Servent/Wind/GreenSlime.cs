using System.Collections;

public class GreenSlime : ServentCardData
{
    public GreenSlime()
    {
        cardNum = "104";
        cardName = "그린 슬라임";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Wind;
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator AttackEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator DefendEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator DeathEffectExecute(BattleManager bm)
	{
		yield return null;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}
