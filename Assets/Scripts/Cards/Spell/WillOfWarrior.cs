using System.Collections;

public class WillOfWarrior : SpellCardData
{
    public WillOfWarrior()
    {
        cardNum = "108";
        cardName = "투사의 의지";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardStoryDesc = "달의 기사는 왕국을 지키기 위해 어둠에 맞선다.";
        cardDesc = "소환된 빛 속성 소환수들은 포스를 1 얻는다.";
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

		yield return null;
	}

}