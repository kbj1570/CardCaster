using System.Collections;

public class DespairOfBerserker : SpellCardData
{
    public DespairOfBerserker()
    {
        cardNum = "19";
        cardName = "절규하는 투사";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardGuideDescription = ""; 
        cardAbility = "자신의 소환수 하나의 포스를 2배로 한다. 그 소환수를 광란 상태로 한다.";
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