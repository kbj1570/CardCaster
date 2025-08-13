using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBless : SpellCardData
{
    public BrokenBless()
    {
        cardNum = "133";
        cardName = "깨어진 축복";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardStoryDesc = "위대한 존재의 축복은 찬란했으나, 그 힘은 인간이 감당하기엔 지나치게 강대했다.\r\n신성한 빛이 스며드는 순간, 그의 육신은 서서히 붕괴하기 시작했다.";
        cardDesc = "소환수 하나의 포스를 2배로 하고 광란 상태로 한다.";
        cardTargetType = ECardTargetType.Selected;
		spellType = ESpellType.Normal;


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

		yield return null;
	}
}