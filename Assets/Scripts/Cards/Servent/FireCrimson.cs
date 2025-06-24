using System.Collections;
using UnityEngine;

public class FireCrimson : ServentCardData
{
    public FireCrimson()
    {
        cardNum = "13";
        cardName = "불의 정령 크림슨";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "장난치는 것을 좋아하는 붉은 색의 정령. 물을 좋아하는 친구와 항상 같이 붙어다닌다.";
        cardAbility = "소환시 덱에서 [물의 정령 헤이즈]를 1장 가져온다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Fire;


        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.DeckCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.None;
        preRequisite.cardNum = "14";

        preRequisites.Add(preRequisite);
    }
	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		bm.SearchCardInDeck(new WaterHeize());
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