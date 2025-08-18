using System.Collections;

public class Boomsquirrel : ServentCardData
{
    public Boomsquirrel()
    {
        cardNum = "123";
        cardName = "부메랑 다람쥐";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "패로 돌아온다.";
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
