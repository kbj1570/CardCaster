using System.Collections;

public class DoubleEdgedSword : SpellCardData
{
	public DoubleEdgedSword()
	{
		cardNum = "106";
		cardName = "양날의 검";
		cardCost = 1;
		cardType = ECardType.Spell;
		cardStoryDesc = "";
		cardDesc = "1장을 드로우 하고,\r\n자신의 HP를 4 잃는다.";
		cardTargetType = ECardTargetType.NoneTargeting;
		spellType = ESpellType.Normal;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		bm.HealPlayer(1);
		yield return null;
	}
}