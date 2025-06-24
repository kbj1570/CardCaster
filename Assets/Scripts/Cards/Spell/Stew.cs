using System.Collections;

public class Stew : SpellCardData
{
	public Stew()
	{
		cardNum = "7";
		cardName = "스튜";
		cardCost = 0;
		cardType = ECardType.Spell;
		cardStoryDesc = "";
		cardAbility = "자신의 HP를 1 회복한다.";
		cardTargetType = ECardTargetType.Selected;

		preRequisites = new();
		PreRequisite preRequisite = new();
		preRequisite.preRequisite = EPreRequisite.None;

		preRequisites.Add(preRequisite);
	}

	public override bool IsSpellUsable(BattleManager bm)
	{
		return true;
	}

	public override IEnumerator SpellEffectExecute(BattleManager bm)
	{
		bm.HealPlayer(1);
		yield return null;
	}



}