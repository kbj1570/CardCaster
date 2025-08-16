using System.Collections;
using System.Collections.Generic;

public class GloriousVictory : SpellCardData
{
	public GloriousVictory()
	{
		cardNum = "131";
		cardName = "영광스러운 승리";
		cardCost = 0;
		cardType = ECardType.Spell;
		cardStoryDesc = "생과 사가 교차하는 치열한 전장,\r\n수많은 적과 맞서 싸운 기사는 마침내 고된 전투를 마치고, 깃발을 하늘 높이 들어 올린다.\r\n그 순간, 황금빛 태양이 따스한 빛으로 그를 감싸 안았다.";
		cardDesc = "덱에서 [제오르기우스]를 1장 가져온다.";
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