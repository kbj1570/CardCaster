using System.Collections;
using System.Collections.Generic;

public class NoWayToReturn : SpellCardData
{
    public NoWayToReturn()
    {
        cardName = "되돌릴 수 없는 선택";
        cardNum = "107";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardStoryDesc = "";
        cardDesc = "1장 드로우 한다.\r\n 이 턴이 끝날 시,\r\n자신의 패를 전부 버린다.";
        cardTargetType = ECardTargetType.Selected;
    }
	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
        bm.DrawCard();
		yield return null;
	}
	public override IEnumerator EndPhaseEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}