using System.Collections;
using UnityEngine;

public class CookOfDarkness : ServentCardData
{
    public CookOfDarkness()
    {
        cardNum = "9";
        cardName = "암흑요리사";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardAbility = "소환시 자신의 덱에서 [스튜]를 2장 가져온다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.DeckCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.None;
        preRequisite.cardNum = "7";

        preRequisites.Add(preRequisite);
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{

        bm.SearchCardInDeck(new Stew());
        yield return new WaitForSeconds(0.4f);
		bm.SearchCardInDeck(new Stew());
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