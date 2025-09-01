using System.Collections;
using UnityEngine;

public class RunTogether : SpellCardData
{
	public RunTogether()
	{
		cardNum = "135";
		cardName = "달리자, 우리 둘이서";
		cardCost = 0;
		cardType = ECardType.Spell;
		cardStoryDesc = "익명의 요리사가 만든 스튜. 한 입이면 누구든지 미소를 짓게 만든다. 다만 그 재료는 어디서도 본 적 없는 것들이다. ";
		cardDesc = "1장 드로우한다.\r\n1 회복한다.";
		cardTargetType = ECardTargetType.NoneTargeting;
		spellType = ESpellType.Normal;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{

		bm.PlayerTakeDamage(4);
		yield return new WaitForSeconds(0.5f);
		bm.DrawCard();

		yield return null;
	}
}