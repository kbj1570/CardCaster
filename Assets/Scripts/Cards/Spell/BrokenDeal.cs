using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenDeal : SpellCardData
{
    public BrokenDeal()
    {
        cardNum = "11";
        cardName = "무너진 계약";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardStoryDesc = "";
        cardDesc = "3장 드로우한다. 이 턴에 상대는 대미지를 받지 않는다.";
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
        bm.DrawCard();
        yield return new WaitForSeconds(0.3f);
		bm.DrawCard();
		yield return new WaitForSeconds(0.3f);
		bm.DrawCard();
		yield return new WaitForSeconds(0.2f);
		bm.SetEnemyDamageBlock(true);

		yield return null;
	}
}