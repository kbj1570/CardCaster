using System.Collections;

public class Celestia : SpellCardData
{
	public Celestia()
	{
		cardNum = "";
		cardName = "달의 왕국 셀레스티아";
		cardCost = 1;
		cardType = ECardType.Spell;
		cardStoryDesc = "";
		cardDesc = "덱 또는 묘지에서 [크레센트 랜서]를 가져온다.";
		cardTargetType = ECardTargetType.NoneTargeting;
		spellType = ESpellType.Field;
	}

	public override bool IsCardUsable(BattleManager bm)
	{
		return true;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		bm.HealPlayer(1);
		yield return null;
	}
}