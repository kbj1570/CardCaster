using System.Collections;
using UnityEngine;

public class BlueSlime : ServentCardData
{
    public BlueSlime()
    {
        cardNum = "102";
        cardName = "블루 슬라임";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "파란색 액체로 구성되어 있으며, 흐물거리는 몸을 이용해 움직인다. \r\n주로 강가나 시냇물이 있는 지역에서 자주 발견된다. 경계심이 적고 친화력이 뛰어나 다른 생명체들에게 인기가 많으며, \r\n어려움에처한 모험가나 여행자에게 도움을 주는 존재로 알려져 있다.";
        cardDesc = "능력 발동 시 1장 드로우한다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Water;
		hasActivationEffect = true;
	}
	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
        bm.DrawCard();
		yield return new WaitForSeconds(1f);
	}
}