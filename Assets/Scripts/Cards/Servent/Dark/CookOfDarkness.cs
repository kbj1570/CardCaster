using System.Collections;
using UnityEngine;

public class CookOfDarkness : ServentCardData
{
	public CookOfDarkness()
	{
		cardNum = "122";
		cardName = "암흑요리사";
		cardCost = 0;
		cardType = ECardType.Servent;
		serventType = EServentType.Player;
		force = 1;
		cardStoryDesc = "";
		cardDesc = "소환 시 자신의 덱에서 \r\n[수상한 스튜]를 2장 가져온다.";
		serventSize = EServentSize.Small;
		cardTargetType = ECardTargetType.Select;
		serventAttribute = EServentAttribute.Dark;
		abilityType = EAbilityType.Summon;

		preRequisites = new();
		PreRequisite preRequisite = new();
		preRequisite.preRequisite = EPreRequisite.DeckCountOver;
		preRequisite.count = 0;
		preRequisite.cardType = ECardType.None;
		preRequisite.cardNum = "107";

		preRequisites.Add(preRequisite);
	}

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{

		bm.SearchCardInDeck(new OddedStew());
		yield return new WaitForSeconds(0.4f);
		bm.SearchCardInDeck(new OddedStew());
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