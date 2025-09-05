using System.Collections;
using UnityEngine;

public class Executioner: ServentCardData
{
    public Executioner()
    {
        cardNum = "136";
        cardName = "처형자";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "능력 발동 시, 자신의 물 속성\r\n소환수 하나를 선택하고\r\n포스를 0으로 한다.";
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Water;
        hasActivationEffect = true;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		bm.SelectServentOnField();
        yield return new WaitUntil(() => bm.actionFlag);
        Servent servent = bm.GetSelectedServent();
        servent.SetForce(0);
	}
}
