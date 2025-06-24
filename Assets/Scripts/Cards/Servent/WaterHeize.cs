using System.Collections;

public class WaterHeize : ServentCardData
{
    public WaterHeize()
    {
        cardNum = "14";
        cardName = "물의 정령 헤이즈";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "장난치는 것을 좋아하는 푸른 색의 정령. 불을 좋아하는 친구와 항상 같이 붙어다닌다.";
        cardAbility = "소환시 덱에서 [불의 정령 크림슨]를 1장 가져온다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Water;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.DeckCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.None;
        preRequisite.cardNum = "13";

        preRequisites.Add(preRequisite);
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		bm.SearchCardInDeck(new FireCrimson());
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