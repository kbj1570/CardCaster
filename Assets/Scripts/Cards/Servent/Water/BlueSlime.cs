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
        cardStoryDesc = "파란색 액체로 구성되어 있으며, 흐물거리는 몸을 이용해 움직인다.";
        cardDesc = "능력 발동 시, 1장을 드로우하고 포스가 0이 된다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Water;
		hasActivationEffect = true;
	}
	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
        bm.DrawCard();
        Servent servent = bm.clickedServent;
        servent.SetForce(0);
		yield return new WaitForSeconds(0.2f);
	}
}