using System.Collections;

public class KnightOfTheAzure : ServentCardData
{
    public KnightOfTheAzure()
    {
        cardNum = "118";
        cardName = "창해의 기사";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
		cardDesc = "소멸될 때, 덱에서 [홍염의 기사]를 1장 가져온다.";
		abilityType = EAbilityType.Death;
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Water;
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
		bm.SearchCardInDeck(new KnightOfTheRedFlame());
		yield return null;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}