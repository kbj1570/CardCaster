using System.Collections;
using UnityEngine;

public class PriceOfBlood : SpellCardData
{
    public PriceOfBlood()
    {
        cardNum = "108";
        cardName = "피의 대가";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardStoryDesc = "익명의 요리사가 만든 스튜. 한 입이면 누구든지 미소를 짓게 만든다. 다만 그 재료는 어디서도 본 적 없는 것들이다. "; 
        cardDesc = "자신의 HP를 4 잃는다. 1장 드로우한다.";
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