using System.Collections;
using UnityEngine;

public class KnightOfTheRedFlame : ServentCardData
{
    public KnightOfTheRedFlame()
    {
        cardNum = "117";
        cardName = "홍염의 기사";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 2;
        cardStoryDesc = "";
        cardDesc = "소멸될 시, 덱에서\r\n[창해의 기사]를 가져온다.";
		abilityType = EAbilityType.Death;
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Fire;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.None;

        preRequisites.Add(preRequisite);
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
		bm.SearchCardInDeck(new KnightOfTheAzure());
		yield return null;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}