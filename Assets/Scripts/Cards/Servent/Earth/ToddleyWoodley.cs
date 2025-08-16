using System.Collections;

public class ToddleyWoodley : ServentCardData
{
    public ToddleyWoodley()
    {
        cardNum = "119";
        cardName = "우들리&토들리";
        cardCost = 2;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 5;
        cardStoryDesc = "";
        cardDesc = "공격시 소환사는 2 대미지를 입는다.";
        abilityType = EAbilityType.Attack;
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
		bm.PlayerTakeDamage(2);
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
