using System.Collections;
using UnityEngine;

public class PriceOfBlood : SpellCardData
{
    public PriceOfBlood()
    {
        cardNum = 8;
        cardName = "피의 대가";
        cardCost = 0;
        spellNum = 7;
        cardType = ECardType.Spell;
        cardGuideDescription = ""; 
        cardAbility = "자신의 HP를 4 잃는다. 1장 드로우한다.";
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

		bm.PlayerTakeDamage(4);
		yield return new WaitForSeconds(0.3f);
		bm.DrawCard();

		yield return null;
	}


}