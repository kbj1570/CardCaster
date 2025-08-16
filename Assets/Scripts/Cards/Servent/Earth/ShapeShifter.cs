using System.Collections;

public class ShapeShifter : ServentCardData
{
    public ShapeShifter()
    {
        cardNum = "112";
        cardName = "셰이프 시프터";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 3;
        cardStoryDesc = "";
        cardDesc = "";
        abilityType = EAbilityType.Summon;
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Earth;
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
