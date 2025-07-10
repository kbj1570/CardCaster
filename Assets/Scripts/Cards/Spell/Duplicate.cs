using System.Collections;
using System.Collections.Generic;

public class Duplicate : SpellCardData
{
    public Duplicate()
    {
        cardNum = "2";
        cardName = "듀플리케이트";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardStoryDesc = "";
        cardDesc = "자신의 소환수 하나를 선택하고 2장 복사해서 덱에 넣는다";
        cardTargetType = ECardTargetType.Select;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.SelectedServent;
        preRequisite.serventAttribute = EServentAttribute.None;

        preRequisites.Add(preRequisite);
    }

	public override bool IsSpellUsable(BattleManager bm)
	{
		return true;
	}

	public override IEnumerator SpellEffectExecute(BattleManager bm)
	{
		bm.DrawCard();
		bm.DrawCard();
		bm.DrawCard();

		yield return null;
	}

}