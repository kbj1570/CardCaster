using System.Collections;

public class OddedStew : SpellCardData
{
	public OddedStew()
	{
		cardNum = "105";
		cardName = "수상한 스튜";
		cardCost = 1;
		cardType = ECardType.Spell;
		cardStoryDesc = "";
		cardDesc = "자신의 HP를 1 회복한다.";
		cardTargetType = ECardTargetType.Selected;
		spellType = ESpellType.Normal;
	}
	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		bm.HealPlayer(1);
		yield return null;
	}
}