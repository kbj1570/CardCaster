using System.Collections;
using UnityEngine;

public class ShapeShifter : ServentCardData
{
    public ShapeShifter()
    {
        cardNum = "112";
        cardName = "셰이프 시프터";
        cardCost = 2;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "능력 발동 시, 소환수 하나를 선택하고 소멸시킨다.";
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Earth;
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
