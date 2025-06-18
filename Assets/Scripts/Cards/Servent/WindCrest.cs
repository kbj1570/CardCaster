using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WindCrest : ServentCardData
{
    public WindCrest()
    {
        cardNum = "16";
        cardName = "바람의 정령 크래스트";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardGuideDescription = "";
        cardAbility = "소환시 자신의 바람 속성 소환수들의 포스를 1 상승시킨다";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Wind;



        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.PlayerServentCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.None;
        preRequisite.serventAttribute = EServentAttribute.Wind;

        preRequisites.Add(preRequisite);
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		List<Field> playerFields = bm.GetPlayerFields();

		foreach (Field field in playerFields)
		{
			if (!field.GetFilled()) continue;

			if (field.GetServentAttribute() == EServentAttribute.Wind)
			{
				field.GainForce(1);
				yield return new WaitForSeconds(0.1f);
			}
		}
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