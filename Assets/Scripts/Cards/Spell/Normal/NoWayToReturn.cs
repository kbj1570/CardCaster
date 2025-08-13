using System.Collections;
using System.Collections.Generic;

public class NoWayToReturn : SpellCardData
{
    public NoWayToReturn()
    {
        cardName = "돌이킬 수 없는 선택";
        cardNum = "107";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardStoryDesc = "구원인가 재앙인가 그것은 중요하지 않았다. \r\n 남는 건 불길과 비명, 그리고 빛나는 무언가뿐이다.";
        cardDesc = "1장 드로우 한다. \r\n 이 턴이 끝날 때 자신의 패를 전부 버린다.";
        cardTargetType = ECardTargetType.Selected;
    }

	public override bool IsSpellUsable(BattleManager bm)
	{
        return true;
	}

	public override IEnumerator SpellEffectExecute(BattleManager bm)
	{
        bm.DrawCard();
		yield return null;
	}
}