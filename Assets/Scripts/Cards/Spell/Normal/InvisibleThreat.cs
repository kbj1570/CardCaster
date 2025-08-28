using System.Collections;

public class InvisibleThreat : SpellCardData
{
    public InvisibleThreat()
    {
        cardNum = "";
        cardName = "보이지 않는 위협";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardStoryDesc = "숨죽이고 있던 어둠이 깨어나기 시작한다.";
        cardDesc = "소환된 빛 속성 소환수들은 포스를 1 얻는다.";
        cardTargetType = ECardTargetType.Select;
		spellType = ESpellType.Normal;

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

		yield return null;
	}
	public override IEnumerator EndPhaseEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}