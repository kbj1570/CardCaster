using System.Collections;

public class BurningSouls : ServentCardData
{
	public BurningSouls()
	{
		cardNum = "126";
		cardName = "불타는 영혼";
		cardCost = 2;
		cardType = ECardType.Servent;
		serventType = EServentType.Player;
		force = 2;
		cardStoryDesc = "";
		cardDesc = "소멸시 상대 소환수 중 무작위로 하나에게 3 대미지를 준다.";
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
	public override IEnumerator DeathEffectExecute(BattleManager bm)
	{
		yield return null;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}