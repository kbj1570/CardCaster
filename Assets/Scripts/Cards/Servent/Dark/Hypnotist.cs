using System.Collections;
using UnityEngine;

public class Hypnotist : ServentCardData
{
    public Hypnotist()
    {
        cardNum = "125";
        cardName = "최면술사";
        cardCost = 3;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 3;
        cardStoryDesc = "";
        cardDesc = "이 소환수가 다른 소환수를 공격할 때, 모든 적에게 대미지 1을 준다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;

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
	public override bool IsAbilityUsable(BattleManager bm)
	{
		return true;
	}
}