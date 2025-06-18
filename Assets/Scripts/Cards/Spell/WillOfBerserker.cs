using System.Collections;

public class WillOfBerserker : SpellCardData
{
    public WillOfBerserker()
    {
        cardNum = "17";
        cardName = "투사의 의지";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardGuideDescription = ""; 
        cardAbility = "자신의 소환수 하나의 포스를 2배로 한다. 그 소환수는 이 턴이 끝나면 소멸한다.";
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