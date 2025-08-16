using System.Collections;

public class Frillizard : ServentCardData
{
    public Frillizard()
    {
        cardNum = "113";
        cardName = "목도리도마뱀";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "척박한 사막지대에서 서식하는 도마뱀.\r\n먹이감을 포착하면 목에있는 프릴을 펼치면서 쫓아온다.";
        cardDesc = "소환 시 상대 소환수 1마리를 혼란 상태로 만든다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;
		abilityType = EAbilityType.Summon;
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
