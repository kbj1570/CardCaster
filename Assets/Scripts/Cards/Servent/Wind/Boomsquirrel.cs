using System.Collections;
using UnityEngine;

public class Boomsquirrel : ServentCardData
{
    public Boomsquirrel()
    {
        cardNum = "123";
        cardName = "부메랑 다람쥐";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "능력 발동 시,\r\n패로 돌아온다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Wind;
        hasActivationEffect = true;
	}
	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
        bm.DrawCard();
		yield return null;
	}
}
