using System.Collections;

public class AtTheEdgeOfPledge : SpellCardData
{
	public AtTheEdgeOfPledge()
	{
		cardNum = "134";
		cardName = "맹세의 칼끝에서";
		cardCost = 1;
		cardType = ECardType.Spell;
		cardStoryDesc = "왕 앞에 무릎 꿇은 그는, 왕국의 정식 기사가 되었다.\r\n검이 어깨를 스치는 순간. 그는 맹세한다.\r\n-나의 검은 모두를 위한 검이니.-";
		cardDesc = "선택한 소환수는 포스를 1 얻는다.그 소환수가 빛 속성이라면 포스를 2 얻는다.";
		cardTargetType = ECardTargetType.Selected;
		spellType = ESpellType.Normal;

		preRequisites = new();
		PreRequisite preRequisite = new();
		preRequisite.preRequisite = EPreRequisite.None;

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