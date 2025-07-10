using System.Collections;

public class AbyssSeeker : ServentCardData
{
    public AbyssSeeker()
    {
        cardNum = "18";
        cardName = "심연의 탐구자";
        cardCost = 2;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 5;
        cardStoryDesc = "";
        voidWalker = true;
        cardDesc = "이 소환수는 다른 소환수나 마법의 효과를 받지 않는다";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;
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