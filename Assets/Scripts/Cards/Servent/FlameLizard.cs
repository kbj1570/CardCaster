using System.Collections;

public class FlameLizard : ServentCardData
{
    public FlameLizard()
    {
        cardNum = "10";
        cardName = "플레임리자드";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardAbility = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Fire;
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
	public override bool IsAbilityUsable(BattleManager bm)
	{
		return true;
	}
}