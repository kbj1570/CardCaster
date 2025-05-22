using System.Collections;
using System.Collections.Generic;

public class VioletLichLord : ServentCardData
{
    public VioletLichLord()
    {
        cardNum = 4;
        serventNum = 1;
        cardName = "바이올렛 리치로드";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        serventAttribute = EServentAttribute.Dark;
        force = 1;
        cardGuideDescription = "";
        cardAbility = "소환시 묘지에서 원하는 마법카드를 1장 가져온다";
        cardTargetType = ECardTargetType.Select;
        
        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.TrashCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.Spell;
        preRequisites.Add(preRequisite);
    }
	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator AttackEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator DefendEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override bool IsAbilityUsable(BattleManager bm)
	{
		return true;
	}
}