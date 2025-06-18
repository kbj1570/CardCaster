using System.Collections;
using System.Collections.Generic;

public class BannedOne : SpellCardData
{
    public BannedOne()
    {
        cardName = "금지된 자";
        cardCost = 99;
        cardType = ECardType.Spell;
        cardGuideDescription = "";
        cardAbility = "자신의 소환수를 모두 소멸시키고 그 수만큼 드로우한다";
        cardTargetType = ECardTargetType.Select;
    }

	public override bool IsSpellUsable(BattleManager bm)
	{
		throw new System.NotImplementedException();
	}

	public override IEnumerator SpellEffectExecute(BattleManager bm)
	{
		throw new System.NotImplementedException();
	}
}