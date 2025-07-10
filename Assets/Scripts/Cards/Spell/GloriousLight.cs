using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;

public class GloriousLight : SpellCardData
{
	public GloriousLight()
	{
		cardNum = "3";
		cardName = "악을 멸하는 등불";
		cardCost = 1;
		cardType = ECardType.Spell;
		cardStoryDesc = "";
		cardDesc = "자신의 빛 속성 소환수가 있을 때 사용할 수 있다. 어둠 소환수들을 전부 소멸시킨다";
		cardTargetType = ECardTargetType.Selected;
		
		preRequisites = new();
		PreRequisite preRequisite = new();
		preRequisite.preRequisite = EPreRequisite.AllServentCountOver;
		preRequisite.count = 0;
		preRequisite.serventAttribute = EServentAttribute.Dark;
		preRequisites.Add(preRequisite);

		preRequisite = new();
		preRequisite.preRequisite = EPreRequisite.PlayerServentCountOver;
		preRequisite.count = 0;                                                                 
		preRequisite.serventAttribute = EServentAttribute.Light;
		
		preRequisites.Add(preRequisite);
	}


	public override bool IsSpellUsable(BattleManager bm)
	{
		List<Field> playerFields = bm.GetPlayerFields();

		foreach (Field field in playerFields)
		{
			if (!field.GetFilled()) continue;

			if (field.GetServentAttribute() == EServentAttribute.Light)
			{ return true; }
		}
		return false;
	}

	public override IEnumerator SpellEffectExecute(BattleManager bm)
	{
		List<Field> playerFields = bm.GetPlayerFields();

		foreach (Field field in playerFields)
		{
			if (!field.GetFilled()) continue;

			if (field.GetServentAttribute() == EServentAttribute.Dark)
			{ field.Kill(); }
		}

		yield return null;
	}

}