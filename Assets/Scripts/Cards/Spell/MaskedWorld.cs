using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class MaskedWorld : SpellCardData
{
	public MaskedWorld()
	{
		cardNum = "15";
		cardName = "마스크월드";
		cardCost = 1;
		cardType = ECardType.Spell;
		cardStoryDesc = ""; 
		cardAbility = "모든 소환수들의 포스를 1 증가시킨다.";
		cardTargetType = ECardTargetType.Selected;

		preRequisites = new();
		PreRequisite preRequisite = new();
		preRequisite.preRequisite = EPreRequisite.AllServentCountOver;
		preRequisite.count = 0;
		preRequisite.cardType = ECardType.None;

		preRequisites.Add(preRequisite);
	}
	public override bool IsSpellUsable(BattleManager bm)
	{
		List<Field> allFields = bm.GetAllFields();

		foreach (Field field in allFields)
		{
			if (field.GetFilled())
				return true;
		}
		return false;
	}

	public override IEnumerator SpellEffectExecute(BattleManager bm)
	{
		List<Field> allFields = bm.GetAllFields();
		foreach (Field field in allFields)
		{
			if (!field.GetFilled()) continue;

			field.GainForce(1);
		}

		yield return null;
	}

}