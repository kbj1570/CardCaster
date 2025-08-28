
using System.Collections;

public class NoPainNoGain : SpellCardData
{
    public NoPainNoGain()
    {
        cardNum = "5";
        cardName = "작은 것을 위한 희생";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardStoryDesc = "";
        cardDesc = "자신의 묘지의 카드를 전부 덱으로 되돌린다.\r\n그 수만큼 자신은 HP를 잃는다.";
        cardTargetType = ECardTargetType.Selected;

		spellType = ESpellType.Normal;
		preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.TrashCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.None;

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
	public override IEnumerator EndPhaseEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}